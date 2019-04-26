using Common;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace HmmNameParser.Tests
{
    public class AddressFormatterTest
    {
        private AddressFormatter Formatter;

        public AddressFormatterTest()
        {
            Formatter = new AddressFormatter();
        }

        [Fact]
        public void Format_ShouldReturnNullIfAddressIsNull()
        {
            Assert.Null(Formatter.Format(null));
        }

        [Theory]
        [ClassData(typeof(TestData))]
        public void Format_ShouldFormatAddressCorrectly(Address input, Address expected)
        {
            Assert.Equal(expected, Formatter.Format(input), new AddressComparer());
        }

        [Theory]
        [InlineData("va", "VA")]
        [InlineData("Ny", "NY")]
        [InlineData("tX", "TX")]
        public void Format_ShouldUpperCaseState(string state, string expected)
        {
            Address input = new Address() { State = state };
            Assert.Equal(expected, Formatter.Format(input).State);
        }

        [Theory]
        [InlineData("1St", "1st")]
        [InlineData("2nD", "2nd")]
        [InlineData("3rd", "3rd")]
        [InlineData("4TH", "4th")]
        public void Format_ShouldLowerCaseOrdinalsInStreetFields(string ordinal, string expected)
        {
            Address input = new Address()
            {
                Street = ordinal,
                AdditionalLine1 = ordinal,
                AdditionalLine2 = ordinal
            };
            Address output = Formatter.Format(input);
            Assert.Equal(expected, output.Street);
            Assert.Equal(expected, output.AdditionalLine1);
            Assert.Equal(expected, output.AdditionalLine2);
        }

        [Theory]
        [InlineData("po", "PO")]
        [InlineData("p.o.", "P.O.")]
        [InlineData("nW", "NW")]
        [InlineData("sW", "SW")]
        [InlineData("Ne", "NE")]
        [InlineData("SE", "SE")]
        public void Format_ShouldUpperCaseExceptionsInStreetFields(string exception, string expected)
        {
            Address input = new Address()
            {
                Street = exception,
                AdditionalLine1 = exception,
                AdditionalLine2 = exception
            };
            Address output = Formatter.Format(input);
            Assert.Equal(expected, output.Street);
            Assert.Equal(expected, output.AdditionalLine1);
            Assert.Equal(expected, output.AdditionalLine2);
        }

        private class TestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                List<object[]> data = new List<object[]>()
                {
                    new object[]
                    {
                        new Address()
                        {
                            Street = "123 elm st.",
                            AdditionalLine1 = "apt. 34",
                            AdditionalLine2 = "floor 3",
                            City = "new york",
                            State = "ny",
                            Zip = "12345",
                            Plus4 = "6789"
                        },
                        new Address()
                        {
                            Street = "123 Elm St.",
                            AdditionalLine1 = "Apt. 34",
                            AdditionalLine2 = "Floor 3",
                            City = "New York",
                            State = "NY",
                            Zip = "12345",
                            Plus4 = "6789"
                        }
                    },
                    new object[]
                    {
                        new Address()
                        {
                            Street = "1593 sPRing Hill rd. nw",
                            AdditionalLine2 = "po box 444",
                            City = "vieNna",
                            State = "VA"
                        },
                        new Address()
                        {
                            Street = "1593 Spring Hill Rd. NW",
                            AdditionalLine2 = "PO Box 444",
                            City = "Vienna",
                            State = "VA"
                        }
                    },
                    new object[]
                    {
                        new Address()
                        {
                            Zip = "55555",
                            Plus4 = "4444"
                        },
                        new Address()
                        {
                            Zip = "55555",
                            Plus4 = "4444"
                        }
                    },
                    new object[]
                    {
                        new Address()
                        {
                            Street = "#88 N 3RD st.",
                            City = "Oklahoma city",
                            State = "OK"
                        },
                        new Address()
                        {
                            Street = "#88 N 3rd St.",
                            City = "Oklahoma City",
                            State = "OK"
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
}
