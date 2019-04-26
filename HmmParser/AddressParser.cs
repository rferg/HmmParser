using Common;
using System;

namespace HmmParser
{
    /// <summary>
    /// Provides method for parsing full address <see cref="string"/> into <see cref="Address"/> class.
    /// </summary>
    public class AddressParser : IParser<Address>
    {
        private IHiddenMarkovModelWrapper HMM;
        private ITagger Tagger;
        private IFormatter<Address> Formatter;

        /// <summary>
        /// Initializes <see cref="AddressParser"/>
        /// </summary>
        public AddressParser() : this(new AddressTagger(), new ModelLoader<AddressParser>(), new AddressFormatter())
        {
        }

        internal AddressParser(ITagger tagger, IModelLoader modelLoader, IFormatter<Address> formatter)
        {
            Tagger = tagger;
            HMM = modelLoader.LoadHMM();
            Formatter = formatter;
        }

        /// <summary>
        /// Parses <paramref name="address"/> into its component parts and returns <see cref="Address"/>.
        /// </summary>
        /// <param name="address">address <see cref="string"/> to parse</param>
        /// <returns>parsed and formatted <see cref="Address"/></returns>
        public Address Parse(string address)
        {
            string[] words = Preprocessing.GetFormattedWords(
                address?.Trim() ?? "",
                new char[1] { '-' });
            int[] tags = Tagger.TagInput(words);
            int[] labels = HMM.Decide(tags);
            Address parsedAddress = AssignToAddressFromLabels(labels, words);
            return Formatter.Format(parsedAddress);
        }

        private Address AssignToAddressFromLabels(int[] labels, string[] words)
        {
            if (labels.Length != words.Length)
            {
                throw new Exception("Number of labels does not match number of words.");
            }

            Address address = new Address();
            for (int i = 0; i < labels.Length; i++)
            {
                AddressLabel label = (AddressLabel)labels[i];
                string word = words[i]?.Trim();

                switch (label)
                {
                    case AddressLabel.Street:
                        address.Street += string.IsNullOrEmpty(address.Street) ? word : " " + word;
                        break;
                    case AddressLabel.AdditionalLine1:
                        address.AdditionalLine1 += string.IsNullOrEmpty(address.AdditionalLine1) ? word : " " + word;
                        break;
                    case AddressLabel.AdditionalLine2:
                        address.AdditionalLine2 += string.IsNullOrEmpty(address.AdditionalLine2) ? word : " " + word;
                        break;
                    case AddressLabel.City:
                        address.City += string.IsNullOrEmpty(address.City) ? word : " " + word;
                        break;
                    case AddressLabel.State:
                        address.State += string.IsNullOrEmpty(address.State) ? word : " " + word;
                        break;
                    case AddressLabel.Zip:
                        address.Zip += string.IsNullOrEmpty(address.Zip) ? word : " " + word;
                        break;
                    case AddressLabel.Plus4:
                        address.Plus4 += string.IsNullOrEmpty(address.Plus4) ? word : " " + word;
                        break;
                    default:
                        throw new Exception($"Label {label} for '{word}' is invalid.");
                }
            }

            return address;
        }
    }
}
