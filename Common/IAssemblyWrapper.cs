using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Wrapper <see langword="interface"/> for <see cref="System.Reflection.Assembly"/>
    /// so it can be used with dependency injection.
    /// </summary>
    public interface IAssemblyWrapper
    {
        Stream GetManifestResourceStream(string resourceName);

        string[] GetManifestResourceNames();
    }
}
