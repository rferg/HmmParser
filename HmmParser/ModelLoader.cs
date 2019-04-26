using Accord.IO;
using Accord.Statistics.Models.Markov;
using Common;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HmmParser
{
    /// <summary>
    /// Loads serialized model.  Implements <see cref="IModelLoader"/>.
    /// Model .bin file name should be same as name of <see cref="ParserType"/>.
    /// </summary>
    internal class ModelLoader<ParserType> : IModelLoader
    {
        private IAssemblyWrapper Assembly;
        private string ModelFileName;

        /// <summary>
        /// Creates instance of <see cref="ModelLoader"/>.
        /// </summary>
        public ModelLoader() : this(new AssemblyWrapper())
        {
            ModelFileName = $"{typeof(ParserType).Name}.bin";
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
        public IHiddenMarkovModelWrapper LoadHMM()
        {
            string resourceName = Assembly.GetManifestResourceNames()
                .SingleOrDefault(name => name.EndsWith(ModelFileName));

            if (string.IsNullOrEmpty(resourceName))
            {
                throw new Exception($"Could not find resource with name {ModelFileName}.");
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

            return new HiddenMarkovModelWrapper(model);
        }
    }
}