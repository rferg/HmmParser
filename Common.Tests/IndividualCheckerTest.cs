using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Common.Tests
{
    public class IndividualCheckerTest
    {
        private IndividualChecker Checker;
        private Mock<IReferenceLoader> LoaderMock;

        public IndividualCheckerTest()
        {
            LoaderMock = new Mock<IReferenceLoader>();
            LoaderMock.Setup(m => m.GetOrgIndicators())
                .Returns(new string[2] { "inc.", "corp." });
            Checker = new IndividualChecker(LoaderMock.Object);
        }

        [Fact]
        public void IsIndividual_ShouldReturnTrueIfNoWordIsInIndicators()
        {
            string[] input = new string[2] { "john", "smith" };
            Assert.True(Checker.IsIndividual(input));
        }

        [Fact]
        public void IsIndividual_ShouldReturnFalseIfWordIsInIndicators()
        {
            string[] input = new string[3] { "john", "smith", "inc." };
            Assert.False(Checker.IsIndividual(input));
        }
    }
}
