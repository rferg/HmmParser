using Common;
using System;
using Xunit;

namespace HmmNameParser.Validation
{
    
    public class ValidationTests
    {
        private NameParser Parser;

        public ValidationTests()
        {
            Parser = new NameParser();
        }

        [Theory]
        [ClassData(typeof(ValidationData))]
        public void ParserValidation(string input, Name output)
        {
            Assert.Equal(output, Parser.Parse(input), new NameComparer());
        }
    }
}
