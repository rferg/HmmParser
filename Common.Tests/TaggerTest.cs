using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Common.Tests
{
    public class TaggerTest
    {
        public class WithOutReferences
        {
            private Tagger Tagger;
            private Mock<ITaggerReferenceLoader> LoaderMock;

            public WithOutReferences()
            {
                LoaderMock = new Mock<ITaggerReferenceLoader>();
                Tagger = new Tagger(LoaderMock.Object);

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
                Assert.True(Tagger.TagInput(input).All(tag => (Tag)tag == Tag.Unknown));
            }

        }

        public class WithReferences
        {
            private Tagger Tagger;
            private Mock<ITaggerReferenceLoader> LoaderMock;

            public WithReferences()
            {
                LoaderMock = new Mock<ITaggerReferenceLoader>();
                SetUpReferences();

                Tagger = new Tagger(LoaderMock.Object);

            }

            private void SetUpReferences()
            {
                string[] prefixes = new string[3] { "mr", "mrs", "p1" };
                LoaderMock.Setup(f => f.GetPrefixes()).Returns(prefixes);
                string[] suffixes = new string[3] { "jr", "p1", "p2" };
                LoaderMock.Setup(f => f.GetSuffixes()).Returns(suffixes);
                string[] surnames = new string[3] { "smith", "p2", "p3" };
                LoaderMock.Setup(f => f.GetSurnames()).Returns(surnames);
                string[] givens = new string[2] { "john", "p3" };
                LoaderMock.Setup(f => f.GetGivenNames()).Returns(givens);
            }

            [Theory]
            [InlineData("mr", Tag.Prefix)]
            [InlineData("jr", Tag.Suffix)]
            [InlineData("john", Tag.GivenName)]
            [InlineData("smith", Tag.Surname)]
            [InlineData("unknown", Tag.Unknown)]
            public void TagInput_ShouldTagWordIfContainedInReferenceArray(string word, Tag expectedTag)
            {
                Assert.Equal(expectedTag, (Tag)Tagger.TagInput(new string[1] { word }).First());
            }

            [Theory]
            [InlineData("p1", Tag.Prefix)]
            [InlineData("p2", Tag.Suffix)]
            [InlineData("p3", Tag.Surname)]
            public void TagInput_ShouldHaveCorrectTagPriorities(string word, Tag expectedTag)
            {
                Assert.Equal(expectedTag, (Tag)Tagger.TagInput(new string[1] { word }).First());
            }
        }
    }
}
