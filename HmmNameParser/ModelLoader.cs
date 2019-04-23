using Accord.IO;
using Accord.Statistics.Models.Markov;
using Common;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HmmNameParser
{
    /// <summary>
    /// Loads serialized model.  Implements <see cref="IModelLoader"/>.
    /// </summary>
    internal class ModelLoader : IModelLoader
    {
        private IAssemblyWrapper Assembly;
        private const string HMM_FILENAME = "hmm.bin";

        /// <summary>
        /// Creates instance of <see cref="ModelLoader"/>.
        /// </summary>
        public ModelLoader() : this(new AssemblyWrapper())
        {            
        }

        internal ModelLoader(IAssemblyWrapper assembly)
        {
            Assembly = assembly;
        }

        /// <summary>
        /// Loads serialized <see cref="HiddenMarkovModel"/> from resource and returns it.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception">If resource name is not found.</exception>
        /// <exception cref="Exception">If resource is not found.</exception>
        public HiddenMarkovModel LoadHMM()
        {
            string resourceName = Assembly.GetManifestResourceNames()
                .SingleOrDefault(name => name.EndsWith(HMM_FILENAME));

            if (string.IsNullOrEmpty(resourceName))
            {
                throw new Exception($"Could not find resource with name {HMM_FILENAME}.");
            }

            HiddenMarkovModel model = null;
            using (Stream stream = Assembly.GetManifestResourceStream(resourceName))
            {
                model = Serializer.Load<HiddenMarkovModel>(stream);
            }

            if (model == null)
            {
                throw new Exception($"Model resource at {resourceName} not found.");
            }

            return model;
        }
    }
}