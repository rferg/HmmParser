using Accord.Statistics.Models.Markov;

namespace HmmParser
{
    /// <summary>
    /// A wrapper class for <see cref="Accord.Statistics.Models.Markov.HiddenMarkovModel"/>
    /// so that it can be used in dependency injection.
    /// </summary>
    internal class HiddenMarkovModelWrapper : IHiddenMarkovModelWrapper
    {
        private HiddenMarkovModel Model;

        public HiddenMarkovModelWrapper(HiddenMarkovModel model)
        {
            Model = model;
        }

        public int[] Decide(int[] input)
        {
            return Model.Decide(input);
        }
    }
}
