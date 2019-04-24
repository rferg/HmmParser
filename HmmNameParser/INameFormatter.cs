using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HmmNameParser
{
    /// <summary>
    /// Provides methods for formatting <see cref="string"/>s in parsed <see cref="Name"/>
    /// </summary>
    public interface INameFormatter
    {
        /// <summary>
        /// Formats strings in <see cref="Name"/>
        /// </summary>
        /// <param name="name">the <see cref="Name"/> to format</param>
        /// <returns>Original <see cref="Name"/> with formatted property values</returns>
        Name Format(Name name);
    }
}
