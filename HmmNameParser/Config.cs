using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HmmNameParser
{
    internal static class Config
    {
        public const string DEFAULT_MODEL_PATH = @".\model\hmm.bin";
        public const string PREFIX_REF = @".\reference\Prefixes.csv";
        public const string GIVEN_NAME_REF = @".\reference\FirstNames.csv";
        public const string SURNAME_REF = @".\reference\LastNames.csv";
        public const string SUFFIX_REF = @".\reference\Suffixes.csv";
    }
}
