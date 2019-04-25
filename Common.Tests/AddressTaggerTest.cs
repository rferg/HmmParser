using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Common.Tests
{
    public class AddressTaggerTest
    {
        public class WithOutReferences
        {
            private AddressTagger Tagger;
            private Mock<IReferenceLoader> LoaderMock;

            public WithOutReferences()
            {
                LoaderMock = new Mock<IReferenceLoader>();
                Tagger = new AddressTagger(LoaderMock.Object);

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
            private AddressTagger Tagger;
            private Mock<IReferenceLoader> LoaderMock;

            public WithReferences()
            {
                LoaderMock = new Mock<IReferenceLoader>();
                SetUpReferences();

                Tagger = new AddressTagger(LoaderMock.Object);

            }

            private void SetUpReferences()
            {
                string[] boxes = new string[3] { "123", "po", "p0" };
                LoaderMock.Setup(f => f.GetBoxes()).Returns(boxes);
                string[] states = new string[3] { "va", "p1", "p0" };
                LoaderMock.Setup(f => f.GetUSStates()).Returns(states);
                string[] unitTypes = new string[3] { "apt", "p1", "p2" };
                LoaderMock.Setup(f => f.GetUnitTypes()).Returns(unitTypes);
                string[] streetTypes = new string[2] { "st", "p2" };
                LoaderMock.Setup(f => f.GetStreetTypes()).Returns(streetTypes);
            }

            [Theory]
            [InlineData("po", AddressTag.Box)]
            [InlineData("va", AddressTag.State)]
            [InlineData("apt", AddressTag.UnitType)]
            [InlineData("st", AddressTag.StreetType)]            
            [InlineData("unknown", AddressTag.Unknown)]
            public void TagInput_ShouldTagWordIfContainedInReferenceArray(string word, AddressTag expectedTag)
            {
                Assert.Equal(expectedTag, (AddressTag)Tagger.TagInput(new string[1] { word }).First());
            }

            [Theory]
            [InlineData("123", AddressTag.Number)]
            [InlineData("p0", AddressTag.Box)]
            [InlineData("p1", AddressTag.State)]
            [InlineData("p2", AddressTag.UnitType)]
            public void TagInput_ShouldHaveCorrectTagPriorities(string word, AddressTag expectedTag)
            {
                Assert.Equal(expectedTag, (AddressTag)Tagger.TagInput(new string[1] { word }).First());
            }

            [Theory]
            [InlineData("1", true)]
            [InlineData("234569", true)]
            [InlineData("22182", true)]
            [InlineData("23rd", false)]
            [InlineData("test", false)]
            [InlineData("kjsfo394843jo", false)]
            public void TagInput_ShouldTagNumbers(string numStr, bool expectedIsNumber)
            {
                Assert.Equal(
                    expectedIsNumber,
                    (AddressTag)Tagger.TagInput(new string[1] { numStr }).First() == AddressTag.Number);
            }
        }

    }
}
