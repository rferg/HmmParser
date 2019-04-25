using System;
using System.Reflection;
using System.Linq;
using System.IO;

namespace Common
{
    /// <summary>
    /// Retrieves reference arrays for <see cref="Tagger"/>
    /// Implements <see cref="IReferenceLoader"/>
    /// </summary>
    public class ReferenceLoader : IReferenceLoader
    {
        private IAssemblyWrapper Assembly;

        public ReferenceLoader() : this(new AssemblyWrapper())
        {
        }

        internal ReferenceLoader(IAssemblyWrapper assembly)
        {
            Assembly = assembly;
        }

        public string[] GetPrefixes()
        {
            return GetReferenceArray("Prefixes.csv");
        }

        public string[] GetGivenNames()
        {
            return GetReferenceArray("FirstNames.csv");
        }

        public string[] GetSurnames()
        {
            return GetReferenceArray("LastNames.csv");
        }

        public string[] GetSuffixes()
        {
            return GetReferenceArray("Suffixes.csv");
        }

        public string[] GetOrgIndicators()
        {
            return GetReferenceArray("OrgIndicators.csv");
        }

        public string[] GetSurnamePrefixes()
        {
            return GetReferenceArray("Surnameprefixes.csv");
        }

        private string[] GetReferenceArray(string fileName)
        {
            string resourceName = Assembly.GetManifestResourceNames()
                .SingleOrDefault(assemblyName =>
                    assemblyName.Contains("Reference") && assemblyName.EndsWith(fileName));

            if (string.IsNullOrEmpty(resourceName))
            {
                throw new Exception($"Could not find resource with file name {fileName}.");
            }

            string line = "";
            using (Stream stream = Assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                // only need first line
                line = reader.ReadLine();
            }

            if (string.IsNullOrEmpty(line))
            {
                throw new Exception($"Reference resource at {resourceName} has an empty first line.");
            }

            return line.Split(',');
        }
    }
}
