using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Common.Tests
{
    public class NameTaggerTest
    {
        public class WithOutReferences
        {
            private NameTagger Tagger;
            private Mock<IReferenceLoader> LoaderMock;

            public WithOutReferences()
            {
                LoaderMock = new Mock<IReferenceLoader>();
                Tagger = new NameTagger(LoaderMock.Object);

                // return empty reference array by default
                LoaderMock.SetReturnsDefault(new string[0]);
            }

            [Fact]
            public void TagInput_ShouldReturnEmptyArrayIfGivenEmpty()
            {
                string[] input = new string[0];
                int[] expected = new int[0];
                Assert.Equal(expected, Tagger.TagInput(input));
            }

            [Fact]
            public void TagInput_ShouldReturnArrayOfSameLengthAsInput()
            {
                string[] input = new string[3] { "", "", "" };
                Assert.Equal(input.Length, Tagger.TagInput(input).Length);
            }

            [Fact]
            public void TagInput_ShouldTagAsUnknownByDefault()
            {
                string[] input = new string[3] { "abc", "def", "ghi" };
                Assert.True(Tagger.TagInput(input).All(tag => (NameTag)tag == NameTag.Unknown));
            }

        }

        public class WithReferences
        {
            private NameTagger Tagger;
            private Mock<IReferenceLoader> LoaderMock;

            public WithReferences()
            {
                LoaderMock = new Mock<IReferenceLoader>();
                SetUpReferences();

                Tagger = new NameTagger(LoaderMock.Object);

            }

            private void SetUpReferences()
            {
                string[] surnamePrefixes = new string[3] { "van", "de", "p0" };
                LoaderMock.Setup(f => f.GetSurnamePrefixes()).Returns(surnamePrefixes);
                string[] prefixes = new string[4] { "mr", "mrs", "p1", "p0" };
                LoaderMock.Setup(f => f.GetPrefixes()).Returns(prefixes);
                string[] suffixes = new string[3] { "jr", "p1", "p2" };
                LoaderMock.Setup(f => f.GetSuffixes()).Returns(suffixes);
                string[] surnames = new string[3] { "smith", "p2", "p3" };
                LoaderMock.Setup(f => f.GetSurnames()).Returns(surnames);
                string[] givens = new string[2] { "john", "p3" };
                LoaderMock.Setup(f => f.GetGivenNames()).Returns(givens);
            }

            [Theory]
            [InlineData("van", NameTag.SurnamePrefix)]
            [InlineData("mr", NameTag.Prefix)]
            [InlineData("jr", NameTag.Suffix)]
            [InlineData("john", NameTag.GivenName)]
            [InlineData("smith", NameTag.Surname)]
            [InlineData("unknown", NameTag.Unknown)]
            public void TagInput_ShouldTagWordIfContainedInReferenceArray(string word, NameTag expectedTag)
            {
                Assert.Equal(expectedTag, (NameTag)Tagger.TagInput(new string[1] { word }).First());
            }

            [Theory]
            [InlineData("p0", NameTag.SurnamePrefix)]
            [InlineData("p1", NameTag.Prefix)]
            [InlineData("p2", NameTag.Suffix)]
            [InlineData("p3", NameTag.Surname)]
            public void TagInput_ShouldHaveCorrectTagPriorities(string word, NameTag expectedTag)
            {
                Assert.Equal(expectedTag, (NameTag)Tagger.TagInput(new string[1] { word }).First());
            }
        }
    }
}
