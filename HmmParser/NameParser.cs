using Common;
using System;
using System.Linq;

namespace HmmParser
{
    /// <summary>
    /// Provides method for parsing full name <see cref="string"/> into <see cref="Name"/> class.
    /// </summary>
    public class NameParser : IParser<Name>
    {
        private IHiddenMarkovModelWrapper HMM;
        private ITagger Tagger;        
        private IFormatter<Name> Formatter;
        private IIndividualChecker IndividualChecker;

        /// <summary>
        /// Initializes <see cref="NameParser"/>.
        /// </summary>
        public NameParser() : this(
            new NameTagger(),
            new ModelLoader<NameParser>(),
            new NameFormatter(), new IndividualChecker())
        {            
        }

        internal NameParser(
            ITagger tagger,
            IModelLoader modelLoader,
            IFormatter<Name> formatter,
            IIndividualChecker individualChecker)
        {
            Tagger = tagger;            
            HMM = modelLoader.LoadHMM();
            Formatter = formatter;
            IndividualChecker = individualChecker;
        }

        /// <summary>
        /// Parses <paramref name="name"/> into its component parts and returns <see cref="Name"/>.
        /// </summary>
        /// <param name="name">name <see cref="string"/> to parse</param>
        /// <returns>parsed and formatted <see cref="Name"/></returns>
        public Name Parse(string name)
        {
            string[] words = Preprocessing.GetFormattedWords(name?.Trim() ?? "");
            int[] labels = new int[0];
            if (IndividualChecker.IsIndividual(words))
            {
                int[] tags = Tagger.TagInput(words);
                labels = HMM.Decide(tags);
            }
            else
            {
                // if non-individual, just assign everything to LastName
                labels = words.Select(w => (int)NameLabel.LastName).ToArray();
            }
            Name parsedName = AssignToNameFromLabels(labels, words);
            return Formatter.Format(parsedName);
        }

        private Name AssignToNameFromLabels(int[] labels, string[] words)
        {
            if (labels.Length != words.Length)
            {
                throw new Exception("Number of labels does not match number of words.");
            }

            Name name = new Name();
            for (int i = 0; i < labels.Length; i++)
            {
                NameLabel label = (NameLabel)labels[i];
                string word = words[i]?.Trim();

                switch (label)
                {
                    case NameLabel.Prefix:
                        name.Prefix += string.IsNullOrEmpty(name.Prefix) ? word : " " + word;
                        break;
                    case NameLabel.FirstName:
                        name.FirstName += string.IsNullOrEmpty(name.FirstName) ? word : " " + word;
                        break;
                    case NameLabel.MiddleName:
                        name.MiddleName += string.IsNullOrEmpty(name.MiddleName) ? word : " " + word;
                        break;
                    case NameLabel.LastName:
                        name.LastName += string.IsNullOrEmpty(name.LastName) ? word : " " + word;
                        break;
                    case NameLabel.Suffix:
                        name.Suffix += string.IsNullOrEmpty(name.Suffix) ? word : " " + word;
                        break;
                    default:
                        throw new Exception($"Label {label} for '{word}' is invalid.");
                }
            }

            return name;
        }
    }
}
