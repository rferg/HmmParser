using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HmmNameParser.Tests
{
    public class NameFormatterTest
    {
        private NameFormatter Formatter;

        public NameFormatterTest()
        {
            Formatter = new NameFormatter();
        }

        [Fact]
        public void Format_ShouldReturnNullIfNameIsNull()
        {
            Assert.Null(Formatter.Format(null));
        }

        [Theory]
        [InlineData("ii", "II")]
        [InlineData("iii", "III")]
        [InlineData("iV", "IV")]
        [InlineData("v", "V")]
        [InlineData("vi (Ret.)", "VI (Ret.)")]
        [InlineData("xiv", "XIV")]
        public void Format_ShouldFormatSuffixRomanNumeralsCorrectly(string suffix, string expectedSuffix)
        {
            Name name = new Name() { Suffix = suffix };
            Assert.Equal(expectedSuffix, Formatter.Format(name).Suffix);
        }

        [Theory]
        [ClassData(typeof(TestData))]
        public void Format_ShouldFormatNameCorrectly(Name input, Name expected)
        {
            Assert.Equal(expected, Formatter.Format(input), new NameComparer());
        }

        [Theory]
        [InlineData("mcdonald", "McDonald")]
        [InlineData("macdonald", "MacDonald")]
        [InlineData("o'reilly", "O'Reilly")]
        [InlineData("mc donald", "Mc Donald")]
        [InlineData("o' reilly", "O' Reilly")]
        public void Format_ShouldFormatIrishLastNames(string input, string expected)
        {
            Name name = new Name() { LastName = input };
            Assert.Equal(expected, Formatter.Format(name).LastName);
        }

        [Theory]
        [InlineData("smith-jones", "Smith-Jones")]
        [InlineData("smith- jones", "Smith- Jones")]
        [InlineData("smith-jones-taylor", "Smith-Jones-Taylor")]
        public void Format_ShouldFormatHyphenatedLastNames(string input, string expected)
        {
            Name name = new Name() { LastName = input };
            Assert.Equal(expected, Formatter.Format(name).LastName);
        }

        private class TestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                List<object[]> data = new List<object[]>()
                {
                    new object[]
                    {
                        new Name()
                        {
                            Prefix = "mr.",
                            FirstName = "john",
                            MiddleName = "d.",
                            LastName = "smith",
                            Suffix = "jr."
                        },
                        new Name()
                        {
                            Prefix = "Mr.",
                            FirstName = "John",
                            MiddleName = "D.",
                            LastName = "Smith",
                            Suffix = "Jr."
                        }
                    },
                    new object[]
                    {
                        new Name()
                        {
                            Prefix = "lt. col.",
                            FirstName = "john",
                            MiddleName = "d.",
                            LastName = "smith",
                            Suffix = "jr. (ret.)"
                        },
                        new Name()
                        {
                            Prefix = "Lt. Col.",
                            FirstName = "John",
                            MiddleName = "D.",
                            LastName = "Smith",
                            Suffix = "Jr. (Ret.)"
                        }
                    },
                    new object[]
                    {
                        new Name()
                        {
                            FirstName = "jOhN",
                            MiddleName = " D.",
                            LastName = "SmiTh ",
                        },
                        new Name()
                        {
                            FirstName = "John",
                            MiddleName = "D.",
                            LastName = "Smith"
                        }
                    },
                    new object[]
                    {
                        new Name()
                        {
                            FirstName = "Karl",
                            MiddleName = "D.",
                            LastName = "van SmiTh",
                        },
                        new Name()
                        {
                            FirstName = "Karl",
                            MiddleName = "D.",
                            LastName = "Van Smith"
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
