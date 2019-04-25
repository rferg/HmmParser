using Accord.Statistics.Models.Markov;
using Accord.Statistics.Models.Markov.Learning;
using Accord.Statistics.Models.Markov.Topology;
using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrainApp
{
    internal class ModelTrainer<TagEnum, LabelEnum>
    {
        private HiddenMarkovModel HMM;
        private MaximumLikelihoodLearning Teacher;

        public ModelTrainer()
        {
            HMM = new HiddenMarkovModel(
                new Forward(Enum.GetValues(typeof(LabelEnum)).Length),
                Enum.GetValues(typeof(TagEnum)).Length);
            HMM.Algorithm = HiddenMarkovModelAlgorithm.Viterbi;

            Teacher = new MaximumLikelihoodLearning(HMM);
        }

        public HiddenMarkovModel TrainModel(int[][] X, int[][] y)
        {
            Teacher.Learn(X, y);
            return HMM;
        }
    }
}
