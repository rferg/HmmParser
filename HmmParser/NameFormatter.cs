using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Common;

namespace HmmParser
{
    /// <summary>
    /// Implementation of <see cref="IFormatter{Name}"/> for names
    /// </summary>
    public class NameFormatter : IFormatter<Name>
    {
        private TextInfo TextInfo;
        private Regex RomanNumeralRegex;
        private Regex IrishRegex;

        public NameFormatter()
        {
            TextInfo = new CultureInfo("en-US", false).TextInfo;
            RomanNumeralRegex = new Regex(@"^[IVX]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            IrishRegex = new Regex(@"^Ma?c|O'", RegexOptions.Compiled | RegexOptions.IgnoreCase);            
        }

        public Name Format(Name name)
        {
            if (name == null) { return name; }
            foreach (PropertyInfo property in name.GetType().GetProperties())
            {
                string value = property.GetValue(name)?.ToString()?.Trim() ?? "";
                if (!string.IsNullOrEmpty(value))
                {
                    string formatted = "";
                    switch (property.Name)
                    {
                        case nameof(Name.LastName):
                            formatted = FormatLastName(value);
                            break;
                        case nameof(Name.Suffix):
                            formatted = FormatSuffix(value);
                            break;
                        case nameof(Name.Prefix):
                        case nameof(Name.FirstName):
                        case nameof(Name.MiddleName):
                        default:
                            formatted = TextInfo.ToTitleCase(value);
                            break;
                    }
                    property.SetValue(name, formatted);
                }
            }
            return name;
        }

        private string FormatSuffix(string suffix)
        {
            string[] splits = suffix.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", splits.Select(s =>
                RomanNumeralRegex.IsMatch(s) ? s.ToUpper() : TextInfo.ToTitleCase(s)));
        }

        private string FormatLastName(string lastName)
        {
            string[] splits = lastName.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", splits.Select(s =>
            {
                Match irishMatch = IrishRegex.Match(s);
                if (irishMatch.Success)
                {
                    return FormatIrishMatch(s, irishMatch);
                }

                return TextInfo.ToTitleCase(s);
            }));
        }

        private string FormatIrishMatch(string input, Match match)
        {
            int index = match.Length;
            return TextInfo.ToTitleCase(input.Substring(0, index))
                + TextInfo.ToTitleCase(input.Substring(index));
        }
    }
}
