using System;
using System.IO;
using System.Linq;

namespace Common
{
    /// <summary>
    /// Exposes method for tagging words in a name with name-part tags based on provided reference arrays.
    /// This converts array of words into array of <see cref="int"/> that can be passed to HMM.
    /// For example, 'Mr.' might be tagged as <see cref="Tag.Prefix"/>.
    /// </summary>
    public class Tagger
    {
        private string[] Prefixes { get; set; }
        private string[] GivenNames { get; set; }
        private string[] Surnames { get; set; }
        private string[] Suffixes { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="Tagger"/> given paths to reference files.
        /// Reference files should be CSVs with one line and no header.
        /// No casing or formatting is done on reference arrays, so the entries should be lowercase/uppercase depending
        /// on the preprocessing method for inputs.)
        /// </summary>
        /// <param name="prefixRefPath">Path to reference file for <see cref="Tag.Prefix"/>s</param>
        /// <param name="givenNameRefPath">Path to reference file for <see cref="Tag.GivenName"/>s</param>
        /// <param name="surnameRefPath">Path to reference file for <see cref="Tag.Surname"/>s</param>
        /// <param name="suffixRefPath">Path to reference file for <see cref="Tag.Suffix"/>s</param>
        public Tagger(string prefixRefPath, string givenNameRefPath, string surnameRefPath, string suffixRefPath)
        {
            Prefixes = ReadSingleLineCSV(prefixRefPath);
            GivenNames = ReadSingleLineCSV(givenNameRefPath);
            Surnames = ReadSingleLineCSV(surnameRefPath);
            Suffixes = ReadSingleLineCSV(suffixRefPath);
        }

        /// <summary>
        /// Creates an instance of <see cref="Tagger"/> given reference arrays.
        /// No casing or formatting is done on reference arrays, so the entries should be lowercase/uppercase depending
        /// on the preprocessing method for inputs.
        /// </summary>
        /// <param name="prefixes">Reference array for <see cref="Tag.Prefix"/></param>
        /// <param name="givenNames">Reference array for <see cref="Tag.GivenName"/></param>
        /// <param name="surnames">Reference array for <see cref="Tag.Surname"/></param>
        /// <param name="suffixes">Reference array for <see cref="Tag.Suffix"/></param>
        public Tagger(string[] prefixes, string[] givenNames, string[] surnames, string[] suffixes)
        {
            Prefixes = prefixes;
            GivenNames = givenNames;
            Surnames = surnames;
            Suffixes = suffixes;
        }

        /// <summary>
        /// Creates array of <see cref="Tag"/> corresponding to words in input array.
        /// Input should already be split, trimmed, cased, and preprocessed.
        /// </summary>
        /// <param name="input">Array of words in name. Should already be formatted correctly.</param>
        /// <returns><see cref="Tag"/> array</returns>
        public Tag[] TagInput(string[] input)
        {
            return input.Select(str => TagWord(str)).ToArray();
        }

        /// <summary>
        /// Creates array of <see cref="int"/> representing underlying value of <see cref="Tag"/>
        /// corresponding to words in input array.  For convenience so that returned value can be passed
        /// directly to HMM.
        /// Input should already be split, trimmed, cased, and preprocessed.
        /// </summary>
        /// <param name="input">Array of words in name. Should already be formatted correctly.</param>
        /// <returns><see cref="int"/> array</returns>
        public int[] TagInputAsInt(string[] input)
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

        private string[] ReadSingleLineCSV(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Did not find file at {path}.");
            }
            string line = "";
            using (var sr = new StreamReader(File.OpenRead(path)))
            {
                // should only be one line
                line = sr.ReadLine();
            }

            if (string.IsNullOrEmpty(line))
            {
                throw new Exception($"File at {path} has an empty first line.");
            }

            return line.Split(',');
        }
    }
}
