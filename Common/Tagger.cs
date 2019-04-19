using System;
using System.IO;
using System.Linq;

namespace Common
{
    /// <summary>
    /// Exposes method for tagging words in a name with name-part tags based on provided reference arrays.
    /// This converts array of words into array of <see cref="int"/> that can be passed to HMM.
    /// For example, 'Mr.' might be tagged as <see cref="Tag.Prefix"/>.
    /// Implements <see cref="IDisposable"/>.
    /// </summary>
    public class Tagger : IDisposable
    {
        private string[] Prefixes { get; set; }
        private string[] GivenNames { get; set; }
        private string[] Surnames { get; set; }
        private string[] Suffixes { get; set; }
        private bool disposed = false;

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

        ~Tagger()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes the <see cref="Tagger"/> instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                Prefixes = new string[0];
                GivenNames = new string[0];
                Surnames = new string[0];
                Suffixes = new string[0];
            }
            disposed = true;
        }

        /// <summary>
        /// Creates array of <see cref="Tag"/> corresponding to words in input array.
        /// Input should already be split, trimmed, cased, and preprocessed.
        /// </summary>
        /// <param name="input">Array of words in name. Should already be formatted correctly.</param>
        /// <returns><see cref="Tag"/> array</returns>
        public Tag[] TagInput(string[] input)
        {
            return input.Select(str =>
            {
                if (Prefixes.Contains(str))
                {
                    return Tag.Prefix;
                }

                if (Suffixes.Contains(str))
                {
                    return Tag.Suffix;
                }

                if (Surnames.Contains(str))
                {
                    return Tag.Surname;
                }

                if (GivenNames.Contains(str))
                {
                    return Tag.GivenName;
                }

                return Tag.Unknown;
            }).ToArray();
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
            return input.Select(str =>
            {
                if (Prefixes.Contains(str))
                {
                    return (int)Tag.Prefix;
                }

                if (Suffixes.Contains(str))
                {
                    return (int)Tag.Suffix;
                }

                if (Surnames.Contains(str))
                {
                    return (int)Tag.Surname;
                }

                if (GivenNames.Contains(str))
                {
                    return (int)Tag.GivenName;
                }

                return (int)Tag.Unknown;
            }).ToArray();
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
