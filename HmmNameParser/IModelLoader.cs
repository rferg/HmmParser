using Accord.Statistics.Models.Markov;

namespace HmmNameParser
{
    /// <summary>
    /// Loads serialized model.
    /// </summary>
    internal interface IModelLoader
    {
        /// <summary>
        /// Loads serialized <see cref="HiddenMarkovModel"/> from resource and returns it.
        /// </summary>
        /// <returns></returns>
        HiddenMarkovModel LoadHMM();
    }
}