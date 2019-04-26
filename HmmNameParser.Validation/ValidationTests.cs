using Common;
using System;
using Xunit;

namespace HmmNameParser.Validation
{
    
    public class ValidationTests
    {
        private NameParser NameParser;
        private AddressParser AddressParser;

        public ValidationTests()
        {
            NameParser = new NameParser();
            AddressParser = new AddressParser();
        }

        [Theory]
        [ClassData(typeof(NameValidationData))]
        public void NameParserValidation(string input, Name output)
        {
            Assert.Equal(output, NameParser.Parse(input), new NameComparer());
        }

        [Theory]
        [ClassData(typeof(AddressValidationData))]
        public void AddressParserValidation(string input, Address output)
        {
            Assert.Equal(output, AddressParser.Parse(input), new AddressComparer());
        }
    }
}
