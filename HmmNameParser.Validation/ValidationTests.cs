using Common;
using System;
using Xunit;

namespace HmmNameParser.Validation
{
    
    public class ValidationTests
    {
        private Parser Parser;

        public ValidationTests()
        {
            Parser = new Parser();
        }

        [Theory]
        [ClassData(typeof(ValidationData))]
        public void ParserValidation(string input, Name output)
        {
            Assert.Equal(output, Parser.Parse(input), new NameComparer());
        }
    }
}
