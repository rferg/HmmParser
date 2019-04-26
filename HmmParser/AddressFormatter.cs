using Common;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace HmmParser
{
    /// <summary>
    /// Implementation of <see cref="IFormatter{Address}"/> for addresses
    /// </summary>
    public class AddressFormatter : IFormatter<Address>
    {
        private TextInfo TextInfo;
        private string[] UpperCaseExceptions;
        private Regex OrdinalRegex;

        public AddressFormatter()
        {
            TextInfo = new CultureInfo("en-US", false).TextInfo;
            UpperCaseExceptions = new string[6] { "PO", "P.O.", "NW", "SW", "NE", "SE" };
            OrdinalRegex = new Regex(
                @"([0-9]st)|([0-9]th)|([0-9]rd)|([0-9]nd)",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public Address Format(Address address)
        {
            if (address == null) { return address; }
            foreach (PropertyInfo property in address.GetType().GetProperties())
            {
                string value = property.GetValue(address)?.ToString()?.Trim() ?? "";
                if (!string.IsNullOrEmpty(value))
                {
                    string formatted = "";
                    switch (property.Name)
                    {
                        case nameof(Address.Zip):
                        case nameof(Address.Plus4):
                            formatted = value;
                            break;
                        case nameof(Address.State):
                            formatted = TextInfo.ToUpper(value);
                            break;
                        case nameof(Address.Street):
                        case nameof(Address.AdditionalLine1):
                        case nameof(Address.AdditionalLine2):
                            formatted = FormatStreet(value);
                            break;
                        case nameof(Address.City):
                        default:
                            formatted = TextInfo.ToTitleCase(value);
                            break;
                    }
                    property.SetValue(address, formatted);
                }
            }
            return address;
        }

        private string FormatStreet(string street)
        {
            string[] splits = street.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", splits.Select(s =>
            {
                if (UpperCaseExceptions.Contains(TextInfo.ToUpper(s)))
                {
                    return TextInfo.ToUpper(s);
                }
                if (OrdinalRegex.IsMatch(s))
                {
                    return TextInfo.ToLower(s);
                }
                return TextInfo.ToTitleCase(s);
            }));
        }
    }
}
