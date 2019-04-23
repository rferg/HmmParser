using Common;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HmmNameParser.Tests
{
    public class ParserTest
    {
        private Parser Parser;
        private Mock<ITagger> TaggerMock;
        private Mock<IModelLoader> ModelLoaderMock;
        private Mock<IHiddenMarkovModelWrapper> HMMMock;

        public ParserTest()
        {
            TaggerMock = new Mock<ITagger>();
            TaggerMock.SetReturnsDefault(new int[0]);

            HMMMock = new Mock<IHiddenMarkovModelWrapper>();
            HMMMock.SetReturnsDefault(new int[0]);
            ModelLoaderMock = new Mock<IModelLoader>();
            ModelLoaderMock.Setup(m => m.LoadHMM()).Returns(HMMMock.Object);

            Parser = new Parser(TaggerMock.Object, ModelLoaderMock.Object);
        }

        [Fact]
        public void Parse_ShouldThrowIfLabelAndRowArraysHaveDifferentLengths()
        {
            string name = "Mr John Smith";
            // default HMMMock.Decide will return empty array
            Assert.ThrowsAny<Exception>(() => Parser.Parse(name));
        }

        [Fact]
        public void Parse_ShouldThrowIfGetsInvalidLabelFromHMM()
        {
            int invalidLabelValue = Enum.GetValues(typeof(Label))
                .Cast<int>().Max() + 1;
            HMMMock.Setup(m => m.Decide(It.IsAny<int[]>()))
                .Returns(new int[1] { invalidLabelValue });
            string name = "John";

            Assert.ThrowsAny<Exception>(() => Parser.Parse(name));
        }

        [Theory]
        [ClassData(typeof(ParseTestData))]
        public void Parse_ShouldAssignToTheCorrectNameProperties(
            string input, int[] labels, Name expectedOutput)
        {
            HMMMock.Setup(m => m.Decide(It.IsAny<int[]>()))
                .Returns(labels);
            Assert.Equal(expectedOutput, Parser.Parse(input), new NameComparer());
        }

        private class NameComparer : IEqualityComparer<Name>
        {
            public bool Equals(Name x, Name y)
            {
                return x.Prefix == y.Prefix
                    && x.FirstName == y.FirstName
                    && x.MiddleName == y.MiddleName
                    && x.LastName == y.LastName
                    && x.Suffix == y.Suffix;
            }

            public int GetHashCode(Name obj)
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
                    new Name()
                };

                yield return new object[]
                {
                    "Mr",
                    new int[1] { (int)Label.Prefix },
                    new Name()
                    {
                        Prefix = "Mr"
                    }
                };

                yield return new object[]
                {
                    "Mr John Smith",
                    new int[3] { (int)Label.Prefix, (int)Label.FirstName, (int)Label.LastName },
                    new Name()
                    {
                        Prefix = "Mr",
                        FirstName = "John",
                        LastName = "Smith"
                    }
                };

                yield return new object[]
                {
                    "Lt. Col. John Smith",
                    new int[4] { (int)Label.Prefix, (int)Label.Prefix, (int)Label.FirstName, (int)Label.LastName },
                    new Name()
                    {
                        Prefix = "Lt. Col.",
                        FirstName = "John",
                        LastName = "Smith"
                    }
                };

                yield return new object[]
                {
                    "Lt. Col. John J. Smith Jr. (Ret.)",
                    new int[7]
                    {
                        (int)Label.Prefix,
                        (int)Label.Prefix,
                        (int)Label.FirstName,
                        (int)Label.MiddleName,
                        (int)Label.LastName,
                        (int)Label.Suffix,
                        (int)Label.Suffix
                    },
                    new Name()
                    {
                        Prefix = "Lt. Col.",
                        FirstName = "John",
                        MiddleName = "J.",
                        LastName = "Smith",
                        Suffix = "Jr. (Ret.)"
                    }
                };

                yield return new object[]
                {
                    " lT. col. JOHN j. SMiTh Jr. (ReT.) ",
                    new int[7]
                    {
                        (int)Label.Prefix,
                        (int)Label.Prefix,
                        (int)Label.FirstName,
                        (int)Label.MiddleName,
                        (int)Label.LastName,
                        (int)Label.Suffix,
                        (int)Label.Suffix
                    },
                    new Name()
                    {
                        Prefix = "Lt. Col.",
                        FirstName = "John",
                        MiddleName = "J.",
                        LastName = "Smith",
                        Suffix = "Jr. (Ret.)"
                    }
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }

}
