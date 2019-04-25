using Common;
using System;
using System.Linq;
using Xunit;
using static Common.Preprocessing;

namespace Common.Tests
{
    public class PreprocessingTest
    {
        [Fact]
        public void GetFormattedWords_ShouldReturnEmptyArrayIfPassedNull()
        {
            Assert.Equal(new string[0] { }, GetFormattedWords(null));
        }

        [Fact]
        public void GetFormattedWords_ShouldReturnEmptyArrayIfPassedEmptyString()
        {
            Assert.Equal(new string[0] { }, GetFormattedWords(string.Empty));
        }

        [Fact]
        public void GetFormattedWords_ShouldLowerCaseAllWords()
        {
            string input = "JoHn DAVID smith Jr.";
            string[] expected = new string[4] { "john", "david", "smith", "jr." };
            Assert.Equal(expected, GetFormattedWords(input));
        }

        [Fact]
        public void GetFormattedWords_ShouldRemoveCommas()
        {
            string input = "Mr. John, David, Jr.";
            Assert.True(
                !GetFormattedWords(input).Any(word => word.Contains(",")));
        }

        [Fact]
        public void GetFormattedWords_ShouldRemoveEmptyEntries()
        {
            string input = "Mr.  John  Smith ";
            string[] expected = new string[3] { "mr.", "john", "smith" };
            Assert.Equal(expected, GetFormattedWords(input));
        }

        [Fact]
        public void GetFormattedWords_ShouldSplitOnWhitespace()
        {
            string input = @"Mr.    John
                                Smith";
            string[] expected = new string[3] { "mr.", "john", "smith" };
            Assert.Equal(expected, GetFormattedWords(input));
        }

        [Fact]
        public void GetFormattedWords_ShouldReturnEmptyArrayIfWhitespaceOnly()
        {
            string input = @"       ";
            string[] expected = new string[0];
            Assert.Equal(expected, GetFormattedWords(input));
        }

        [Fact]
        public void GetFormattedWords_ShouldSplitOnProvidedCharsAndWhitespace()
        {
            string input = @"a b,c-d, e f
                                g";
            string[] expected = new string[7] { "a", "b", "c", "d", "e", "f", "g" };
            char[] splitChars = new char[2] { ',', '-' };
            Assert.Equal(expected, GetFormattedWords(input, splitChars));
        }
    }
}
