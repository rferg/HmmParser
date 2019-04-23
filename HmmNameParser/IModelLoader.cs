using Accord.Statistics.Models.Markov;

namespace HmmNameParser
{
    /// <summary>
    /// Loads serialized model.
    /// </summary>
    public interface IModelLoader
    {
        /// <summary>
        /// Loads serialized <see cref="HiddenMarkovModel"/> from resource and returns it.
        /// </summary>
        /// <returns></returns>
        IHiddenMarkovModelWrapper LoadHMM();
    }
}