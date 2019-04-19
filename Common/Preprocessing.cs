using System;
using System.Linq;

namespace Common
{
    /// <summary>
    /// <see langword="static"/> class that provides methods for converting
    /// name input <see cref="string"/> into format that can be tagged by <see cref="Tagger"/>
    /// </summary>
    public static class Preprocessing
    {
        /// <summary>
        /// Splits and formats name into <see cref="string"/> array that can be tagged by <see cref="Tagger"/>
        /// </summary>
        /// <param name="fullName">The name to be parsed.</param>
        /// <returns>Split and formatted <see cref="string"/> array.</returns>
        public static string[] GetFormattedWords(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                return new string[] { };
            }
            // split on whitespace and remove any empty entries
            return fullName.Split((char[])null, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Replace(",", "").ToLower().Trim())
                .ToArray();
        }
    }
}
