using Common;
using System.Collections.Generic;

namespace HmmParser.Validation
{
    public class AddressComparer : IEqualityComparer<Address>
    {
        public bool Equals(Address x, Address y)
        {
            return x.Street == y.Street
                && x.AdditionalLine1 == y.AdditionalLine1
                && x.AdditionalLine2 == y.AdditionalLine2
                && x.City == y.City
                && x.State == y.State
                && x.Zip == y.Zip
                && x.Plus4 == y.Plus4;
        }

        public int GetHashCode(Address obj)
        {
            return obj.GetHashCode();
        }
    
    }
}
