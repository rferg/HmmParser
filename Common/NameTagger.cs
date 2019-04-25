using System.Linq;

namespace Common
{
    /// <summary>
    /// Exposes method for tagging words in a name with name-part tags based on provided reference arrays.
    /// This converts array of words into array of <see cref="int"/> that can be passed to HMM.
    /// For example, 'Mr.' might be tagged as <see cref="NameTag.Prefix"/>.
    /// Implements <see cref="ITagger"/>
    /// </summary>
    public class NameTagger : ITagger
    {
        private string[] Prefixes;
        private string[] GivenNames;
        private string[] Surnames;
        private string[] Suffixes;
        private string[] SurnamePrefixes;

        /// <summary>
        /// Initializes an instance of <see cref="NameTagger"/>
        /// </summary>
        public NameTagger() : this(new ReferenceLoader())
        {
        }

        internal NameTagger(IReferenceLoader referenceLoader)
        {
            Prefixes = referenceLoader.GetPrefixes();
            GivenNames = referenceLoader.GetGivenNames();
            Surnames = referenceLoader.GetSurnames();
            Suffixes = referenceLoader.GetSuffixes();
            SurnamePrefixes = referenceLoader.GetSurnamePrefixes();
        }

        /// <summary>
        /// Creates array of <see cref="int"/> representing underlying value of <see cref="NameTag"/>
        /// corresponding to words in input array.  For convenience so that returned value can be passed
        /// directly to HMM.
        /// Input should already be split, trimmed, cased, and preprocessed.
        /// </summary>
        /// <param name="input">Array of words in name. Should already be formatted correctly.</param>
        /// <returns><see cref="int"/> array</returns>
        public int[] TagInput(string[] input)
        {
            return input.Select(str => (int)TagWord(str)).ToArray();
        }

        private NameTag TagWord(string word)
        {
            if (SurnamePrefixes.Contains(word))
            {
                return NameTag.SurnamePrefix;
            }

            if (Prefixes.Contains(word))
            {
                return NameTag.Prefix;
            }

            if (Suffixes.Contains(word))
            {
                return NameTag.Suffix;
            }

            if (Surnames.Contains(word))
            {
                return NameTag.Surname;
            }

            if (GivenNames.Contains(word))
            {
                return NameTag.GivenName;
            }

            return NameTag.Unknown;
        }
    }
}
