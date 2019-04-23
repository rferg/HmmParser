using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Wrapper for <see cref="System.Reflection.Assembly"/> to allow for dependency injection.
    /// Implements <see cref="IAssemblyWrapper"/>
    /// </summary>
    public class AssemblyWrapper : IAssemblyWrapper
    {
        private Assembly Assembly;

        public AssemblyWrapper()
        {
            Assembly = Assembly.GetExecutingAssembly();
        }

        public string[] GetManifestResourceNames()
        {
            return Assembly.GetManifestResourceNames();
        }

        public Stream GetManifestResourceStream(string resourceName)
        {
            return Assembly.GetManifestResourceStream(resourceName);
        }
    }
}
