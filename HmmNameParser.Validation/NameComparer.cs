using Common;
using System.Collections.Generic;

namespace HmmNameParser.Validation
{
    internal class NameComparer : IEqualityComparer<Name>
    {
        public bool Equals(Name x, Name y)
        {
            return x.Prefix == y.Prefix
                && x.FirstName == y.FirstName
                && x.MiddleName == y.MiddleName
                && x.LastName == y.LastName
                && x.Suffix == y.Suffix;
        }

        public int GetHashCode(Name obj)
        {
            return obj.GetHashCode();
        }
    }
}
