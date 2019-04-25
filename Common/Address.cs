using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Represents a parsed address.
    /// </summary>
    public class Address
    {
        public string Street { get; set; }
        public string AdditionalLine1 { get; set; }
        public string AdditionalLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Plus4 { get; set; }
    }
}
