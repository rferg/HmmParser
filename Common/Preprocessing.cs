using System;
using System.Linq;

namespace Common
{
    /// <summary>
    /// <see langword="static"/> class that provides methods for converting
    /// input <see cref="string"/> into format that can be tagged by <see cref="ITagger"/>
    /// </summary>
    public static class Preprocessing
    {
        /// <summary>
        /// Splits and formats input <see cref="string"/> into array that can be tagged by <see cref="ITagger"/>
        /// </summary>
        /// <param name="input">The input to be parsed.</param>
        /// <returns>Split and formatted <see cref="string"/> array.</returns>
        public static string[] GetFormattedWords(string input, char[] additionalSplitChars = null)
        {
            if (string.IsNullOrEmpty(input))
            {
                return new string[] { };
            }

            // if splitChars is null, will split on all whitespace
            char[] splitChars = additionalSplitChars?.Concat(new char[3] { ' ', '\t', '\n' }).ToArray();
            // split on whitespace and remove any empty entries
            return input.Split(splitChars, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Replace(",", "").ToLower().Trim())
                .ToArray();
        }
    }
}
