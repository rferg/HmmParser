﻿using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Common.Tests
{
    public class ReferenceLoaderTest
    {
        private ReferenceLoader Loader;
        private Mock<IAssemblyWrapper> AssemblyMock;

        public ReferenceLoaderTest()
        {
            AssemblyMock = new Mock<IAssemblyWrapper>();
            AssemblyMock.Setup(a => a.GetManifestResourceNames())
                .Returns(new string[0]);
            AssemblyMock.Setup(a => a.GetManifestResourceStream(It.IsAny<string>()))
                .Returns(new MemoryStream());

            Loader = new ReferenceLoader(AssemblyMock.Object);
        }

        [Fact]
        public void GetAny_ThrowsIfNoResourceName()
        {
            Assert.ThrowsAny<Exception>(() => Loader.GetPrefixes());
        }

        [Fact]
        public void GetAny_ThrowsIfResourceIsEmpty()
        {
            AssemblyMock.Setup(a =>
                a.GetManifestResourceNames()).Returns(new string[1] { "Reference.Prefixes.csv" });
            Assert.ThrowsAny<Exception>(() => Loader.GetPrefixes());
        }

        private void SetUpAssemblyMock(string fileName, string refString)
        {
            AssemblyMock.Setup(a =>
                a.GetManifestResourceNames()).Returns(new string[1] { $"Reference.{fileName}" });
            AssemblyMock.Setup(a =>
                a.GetManifestResourceStream(It.IsAny<string>()))
                    .Returns(new MemoryStream(Encoding.ASCII.GetBytes(refString)));
        }

        [Fact]
        public void GetPrefixes_ReturnsReferenceArray()
        {
            string refString = "a,b,c";
            string[] expected = refString.Split(',');
            string fileName = "Prefixes.csv";
            SetUpAssemblyMock(fileName, refString);

            Assert.Equal(expected, Loader.GetPrefixes());
        }

        [Fact]
        public void GetGivenNames_ReturnsReferenceArray()
        {
            string refString = "a,b,c";
            string[] expected = refString.Split(',');
            string fileName = "FirstNames.csv";
            SetUpAssemblyMock(fileName, refString);

            Assert.Equal(expected, Loader.GetGivenNames());
        }

        [Fact]
        public void GetSurNames_ReturnsReferenceArray()
        {
            string refString = "a,b,c";
            string[] expected = refString.Split(',');
            string fileName = "LastNames.csv";
            SetUpAssemblyMock(fileName, refString);

            Assert.Equal(expected, Loader.GetSurnames());
        }

        [Fact]
        public void GetSuffixes_ReturnsReferenceArray()
        {
            string refString = "a,b,c";
            string[] expected = refString.Split(',');
            string fileName = "Suffixes.csv";
            SetUpAssemblyMock(fileName, refString);

            Assert.Equal(expected, Loader.GetSuffixes());
        }

        [Fact]
        public void GetSurnamePrefixes_ReturnsReferenceArray()
        {
            string refString = "a,b,c";
            string[] expected = refString.Split(',');
            string fileName = "Surnameprefixes.csv";
            SetUpAssemblyMock(fileName, refString);

            Assert.Equal(expected, Loader.GetSurnamePrefixes());
        }        

        [Fact]
        public void GetStreetTypes_ReturnsReferenceArray()
        {
            string refString = "a,b,c";
            string[] expected = refString.Split(',');
            string fileName = "StreetTypes.csv";
            SetUpAssemblyMock(fileName, refString);

            Assert.Equal(expected, Loader.GetStreetTypes());
        }

        [Fact]
        public void GetUnitTypes_ReturnsReferenceArray()
        {
            string refString = "a,b,c";
            string[] expected = refString.Split(',');
            string fileName = "UnitTypes.csv";
            SetUpAssemblyMock(fileName, refString);

            Assert.Equal(expected, Loader.GetUnitTypes());
        }

        [Fact]
        public void GetBoxes_ReturnsReferenceArray()
        {
            string refString = "a,b,c";
            string[] expected = refString.Split(',');
            string fileName = "Boxes.csv";
            SetUpAssemblyMock(fileName, refString);

            Assert.Equal(expected, Loader.GetBoxes());
        }

        [Fact]
        public void GetUSStates_ReturnsReferenceArray()
        {
            string refString = "a,b,c";
            string[] expected = refString.Split(',');
            string fileName = "States.csv";
            SetUpAssemblyMock(fileName, refString);

            Assert.Equal(expected, Loader.GetUSStates());
        }

        [Fact]
        public void GetCityIndicators_ReturnsReferenceArray()
        {
            string refString = "a,b,c";
            string[] expected = refString.Split(',');
            string fileName = "CityIndicators.csv";
            SetUpAssemblyMock(fileName, refString);

            Assert.Equal(expected, Loader.GetCityIndicators());
        }

        [Fact]
        public void GetDirections_ReturnsReferenceArray()
        {
            string refString = "a,b,c";
            string[] expected = refString.Split(',');
            string fileName = "Directions.csv";
            SetUpAssemblyMock(fileName, refString);

            Assert.Equal(expected, Loader.GetDirections());
        }

        [Fact]
        public void GetCommonStreetNames_ReturnsReferenceArray()
        {
            string refString = "a,b,c";
            string[] expected = refString.Split(',');
            string fileName = "CommonStreetNames.csv";
            SetUpAssemblyMock(fileName, refString);

            Assert.Equal(expected, Loader.GetCommonStreetNames());
        }
    }
}
