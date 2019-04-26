using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Address-part tags that apply to words in address and are inputs to the HMM
    /// </summary>
    public enum AddressTag
    {
        Unknown = 0,
        Number = 1,
        StreetType = 2,
        UnitType = 3,
        Box = 4,
        State = 5,
        CityIndicator = 6,
        Direction = 7
    }
}
