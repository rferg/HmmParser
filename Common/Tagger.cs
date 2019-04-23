﻿using System.Linq;

namespace Common
{
    /// <summary>
    /// Exposes method for tagging words in a name with name-part tags based on provided reference arrays.
    /// This converts array of words into array of <see cref="int"/> that can be passed to HMM.
    /// For example, 'Mr.' might be tagged as <see cref="Tag.Prefix"/>.
    /// Implements <see cref="ITagger"/>
    /// </summary>
    public class Tagger : ITagger
    {
        private string[] Prefixes;
        private string[] GivenNames;
        private string[] Surnames;
        private string[] Suffixes;

        /// <summary>
        /// Initializes an instance of <see cref="Tagger"/>
        /// </summary>
        public Tagger() : this(new TaggerReferenceLoader())
        {
        }

        internal Tagger(ITaggerReferenceLoader referenceLoader)
        {
            Prefixes = referenceLoader.GetPrefixes();
            GivenNames = referenceLoader.GetGivenNames();
            Surnames = referenceLoader.GetSurnames();
            Suffixes = referenceLoader.GetSuffixes();
        }

        /// <summary>
        /// Creates array of <see cref="int"/> representing underlying value of <see cref="Tag"/>
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

        private Tag TagWord(string word)
        {
            if (Prefixes.Contains(word))
            {
                return Tag.Prefix;
            }

            if (Suffixes.Contains(word))
            {
                return Tag.Suffix;
            }

            if (Surnames.Contains(word))
            {
                return Tag.Surname;
            }

            if (GivenNames.Contains(word))
            {
                return Tag.GivenName;
            }

            return Tag.Unknown;
        }
    }
}