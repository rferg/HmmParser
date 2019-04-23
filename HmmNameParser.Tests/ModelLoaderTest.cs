﻿using Common;
using Moq;
using System;
using System.IO;
using Xunit;

namespace HmmNameParser.Tests
{
    public class ModelLoaderTest
    {
        private ModelLoader Loader;
        private Mock<IAssemblyWrapper> AssemblyMock;

        public ModelLoaderTest()
        {
            AssemblyMock = new Mock<IAssemblyWrapper>();
            AssemblyMock.Setup(a => a.GetManifestResourceNames())
                .Returns(new string[0]);
            AssemblyMock.Setup(a => a.GetManifestResourceStream(It.IsAny<string>()))
                .Returns(new MemoryStream());

            Loader = new ModelLoader(AssemblyMock.Object);
        }

        [Fact]
        public void LoadHMM_ThrowsIfNoResourceName()
        {
            Assert.ThrowsAny<Exception>(() => Loader.LoadHMM());
        }

        [Fact]
        public void GetAny_ThrowsIfResourceIsEmpty()
        {
            AssemblyMock.Setup(a =>
                a.GetManifestResourceNames()).Returns(new string[1] { "hmm.bin" });
            Assert.ThrowsAny<Exception>(() => Loader.LoadHMM());
        }
    }
}