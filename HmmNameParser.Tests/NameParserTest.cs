using Common;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HmmNameParser.Tests
{
    public class NameParserTest
    {
        private NameParser Parser;
        private Mock<ITagger> TaggerMock;
        private Mock<IModelLoader> ModelLoaderMock;
        private Mock<IHiddenMarkovModelWrapper> HMMMock;
        private Mock<IFormatter<Name>> FormatterMock;
        private Mock<IIndividualChecker> IndividualCheckerMock;

        public NameParserTest()
        {
            TaggerMock = new Mock<ITagger>();
            TaggerMock.SetReturnsDefault(new int[0]);

            HMMMock = new Mock<IHiddenMarkovModelWrapper>();
            HMMMock.SetReturnsDefault(new int[0]);
            ModelLoaderMock = new Mock<IModelLoader>();
            ModelLoaderMock.Setup(m => m.LoadHMM()).Returns(HMMMock.Object);

            FormatterMock = new Mock<IFormatter<Name>>();
            FormatterMock.Setup(f => f.Format(It.IsAny<Name>())).Returns<Name>(name => name);

            IndividualCheckerMock = new Mock<IIndividualChecker>();
            IndividualCheckerMock.SetReturnsDefault(true);

            Parser = new NameParser(
                TaggerMock.Object,
                ModelLoaderMock.Object,
                FormatterMock.Object,
                IndividualCheckerMock.Object);
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
            int invalidLabelValue = Enum.GetValues(typeof(NameLabel))
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
            Assert.Equal(expectedOutput, Parser.Parse(input), new CaseInsensitiveNameComparer());
        }

        [Fact]
        public void Parse_ShouldAssignWholeNameToLastNameWhenNonIndividual()
        {
            IndividualCheckerMock.Setup(m => m.IsIndividual(It.IsAny<string[]>())).Returns(false);
            string input = "A Corporation Inc.";
            Name expected = new Name() { LastName = input };
            Assert.Equal(expected, Parser.Parse(input), new CaseInsensitiveNameComparer());
        }

        private class CaseInsensitiveNameComparer : IEqualityComparer<Name>
        {
            public bool Equals(Name x, Name y)
            {
                // formatting is handled by INameFormatter class
                // so need to test that here
                return x.Prefix?.ToLower() == y.Prefix?.ToLower()
                    && x.FirstName?.ToLower() == y.FirstName?.ToLower()
                    && x.MiddleName?.ToLower() == y.MiddleName?.ToLower()
                    && x.LastName?.ToLower() == y.LastName?.ToLower()
                    && x.Suffix?.ToLower() == y.Suffix?.ToLower();
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
                    new int[1] { (int)NameLabel.Prefix },
                    new Name()
                    {
                        Prefix = "Mr"
                    }
                };

                yield return new object[]
                {
                    "Mr John Smith",
                    new int[3] { (int)NameLabel.Prefix, (int)NameLabel.FirstName, (int)NameLabel.LastName },
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
                    new int[4] { (int)NameLabel.Prefix, (int)NameLabel.Prefix, (int)NameLabel.FirstName, (int)NameLabel.LastName },
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
                        (int)NameLabel.Prefix,
                        (int)NameLabel.Prefix,
                        (int)NameLabel.FirstName,
                        (int)NameLabel.MiddleName,
                        (int)NameLabel.LastName,
                        (int)NameLabel.Suffix,
                        (int)NameLabel.Suffix
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
                        (int)NameLabel.Prefix,
                        (int)NameLabel.Prefix,
                        (int)NameLabel.FirstName,
                        (int)NameLabel.MiddleName,
                        (int)NameLabel.LastName,
                        (int)NameLabel.Suffix,
                        (int)NameLabel.Suffix
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
