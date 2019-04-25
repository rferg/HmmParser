using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Corresponds to properties of <see cref="Address"/>
    /// </summary>
    public enum AddressLabel
    {
        Street = 0,
        AdditionalLine1 = 1,
        AdditionalLine2 = 2,
        City = 3,
        State = 4,
        Zip = 5,
        Plus4 = 6
    }
}
