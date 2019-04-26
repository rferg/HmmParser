using Common;
using System.Collections;
using System.Collections.Generic;

namespace HmmNameParser.Validation
{
    internal class AddressValidationData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            List<object[]> data = new List<object[]>()
            {
                new object[]
                {
                    "1593 Spring hill Road, Suite 400 Tysons Corner, VA 22182",
                    new Address()
                    {
                        Street = "1593 Spring Hill Road",
                        AdditionalLine1 = "Suite 400",
                        City = "Tysons Corner",
                        State = "VA",
                        Zip = "22182"
                    }
                },
                new object[]
                {
                    "2772 Woodhull Avenue Bronx, NY 10469-6126",
                    new Address()
                    {
                        Street = "2772 Woodhull Avenue",
                        City = "Bronx",
                        State = "NY",
                        Zip = "10469",
                        Plus4 = "6126"
                    }
                },
                new object[]
                {
                    "Po box 180 Greensburg, KY 42743 0180",
                    new Address()
                    {
                        Street = "PO Box 180",
                        City = "Greensburg",
                        State = "KY",
                        Zip = "42743",
                        Plus4 = "0180"
                    }
                },
                new object[]
                {
                    "5200 Sw Lakeshore Drive #240 Tempe, AZ",
                    new Address()
                    {
                        Street = "5200 SW Lakeshore Drive",
                        AdditionalLine1 = "#240",
                        City = "Tempe",
                        State = "AZ"
                    }
                },
                new object[]
                {
                    "975 Bacons Bridge Road Unit 148 Summerville, SC 29485-4189",
                    new Address()
                    {
                        Street = "975 Bacons Bridge Road",
                        AdditionalLine1 = "Unit 148",
                        City = "Summerville",
                        State = "SC",
                        Zip = "29485",
                        Plus4 = "4189"
                    }
                },
                new object[]
                {
                    "1714 5th Street Santa Monica, CA",
                    new Address()
                    {
                        Street = "1714 5th Street",
                        City = "Santa Monica",
                        State = "CA"
                    }
                }
            };

            for (int i = 0; i < data.Count; i++)
            {
                yield return data[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}