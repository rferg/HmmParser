using Common;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HmmParser.Tests
{
    public class AddressParserTest
    {
        private AddressParser Parser;
        private Mock<ITagger> TaggerMock;
        private Mock<IModelLoader> ModelLoaderMock;
        private Mock<IHiddenMarkovModelWrapper> HMMMock;
        private Mock<IFormatter<Address>> FormatterMock;

        public AddressParserTest()
        {
            TaggerMock = new Mock<ITagger>();
            TaggerMock.SetReturnsDefault(new int[0]);

            HMMMock = new Mock<IHiddenMarkovModelWrapper>();
            HMMMock.SetReturnsDefault(new int[0]);
            ModelLoaderMock = new Mock<IModelLoader>();
            ModelLoaderMock.Setup(m => m.LoadHMM()).Returns(HMMMock.Object);

            FormatterMock = new Mock<IFormatter<Address>>();
            FormatterMock.Setup(f => f.Format(It.IsAny<Address>())).Returns<Address>(addr => addr);

            Parser = new AddressParser(
                TaggerMock.Object,
                ModelLoaderMock.Object,
                FormatterMock.Object);
        }

        [Fact]
        public void Parse_ShouldThrowIfLabelAndRowArraysHaveDifferentLengths()
        {
            string name = "123 Elm St.";
            // default HMMMock.Decide will return empty array
            Assert.ThrowsAny<Exception>(() => Parser.Parse(name));
        }

        [Fact]
        public void Parse_ShouldThrowIfGetsInvalidLabelFromHMM()
        {
            int invalidLabelValue = Enum.GetValues(typeof(AddressLabel))
                .Cast<int>().Max() + 1;
            HMMMock.Setup(m => m.Decide(It.IsAny<int[]>()))
                .Returns(new int[1] { invalidLabelValue });
            string address = "123";

            Assert.ThrowsAny<Exception>(() => Parser.Parse(address));
        }

        [Theory]
        [ClassData(typeof(ParseTestData))]
        public void Parse_ShouldAssignToTheCorrectNameProperties(
            string input, int[] labels, Address expectedOutput)
        {
            HMMMock.Setup(m => m.Decide(It.IsAny<int[]>()))
                .Returns(labels);
            Assert.Equal(expectedOutput, Parser.Parse(input), new CaseInsensitiveAddressComparer());
        }

        private class CaseInsensitiveAddressComparer : IEqualityComparer<Address>
        {
            public bool Equals(Address x, Address y)
            {
                // formatting is handled by IFormatter class
                // so need to test that here
                return x.Street?.ToLower() == y.Street?.ToLower()
                    && x.AdditionalLine1?.ToLower() == y.AdditionalLine1?.ToLower()
                    && x.AdditionalLine2?.ToLower() == y.AdditionalLine2?.ToLower()
                    && x.City?.ToLower() == y.City?.ToLower()
                    && x.State?.ToLower() == y.State?.ToLower()
                    && x.Zip?.ToLower() == y.Zip?.ToLower()
                    && x.Plus4?.ToLower() == y.Plus4?.ToLower();
            }

            public int GetHashCode(Address obj)
            {
                return obj.GetHashCode();
            }
        }

        private class ParseTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    "",
                    new int[0],
                    new Address()
                };

                yield return new object[]
                {
                    "Street",
                    new int[1] { (int)AddressLabel.Street },
                    new Address()
                    {
                        Street = "Street"
                    }
                };

                yield return new object[]
                {
                    "12345",
                    new int[1] { (int)AddressLabel.Zip },
                    new Address()
                    {
                        Zip = "12345"
                    }
                };

                yield return new object[]
                {
                    "123 City, ST",
                    new int[3] { (int)AddressLabel.Street, (int)AddressLabel.City, (int)AddressLabel.State },
                    new Address()
                    {
                        Street = "123",
                        City = "City",
                        State = "ST"
                    }
                };

                yield return new object[]
                {
                    "123 Street Add1 Add2 City City, ST 12345-3456",
                    new int[9] 
                    {
                        (int)AddressLabel.Street,
                        (int)AddressLabel.Street,
                        (int)AddressLabel.AdditionalLine1,
                        (int)AddressLabel.AdditionalLine2,
                        (int)AddressLabel.City,
                        (int)AddressLabel.City,
                        (int)AddressLabel.State,
                        (int)AddressLabel.Zip,
                        (int)AddressLabel.Plus4
                    },
                    new Address()
                    {
                        Street = "123 Street",
                        AdditionalLine1 = "Add1",
                        AdditionalLine2 = "Add2",
                        City = "City City",
                        State = "ST",
                        Zip = "12345",
                        Plus4 = "3456"
                    }
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
