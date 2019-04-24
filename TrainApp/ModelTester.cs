using Accord.Statistics.Models.Markov;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrainApp
{
    internal class ModelTester
    {
        private HiddenMarkovModel HMM;

        public ModelTester(HiddenMarkovModel hmm)
        {
            HMM = hmm;
        }

        public HMMTestResult Test(Tuple<int[][], int[][]> xAndY)
        {
            int[][] predicted = HMM.Decide(xAndY.Item1);
            HMMTestResult result = predicted
                .Zip(xAndY.Item2, (pred, actual) => new HMMTestResult
                {
                    TotalAccuracy = pred.SequenceEqual(actual) ? 1 : 0,
                    AverageAccuracy = pred.Zip(actual, (p, a) => p == a ? 1 : 0).Sum() / (float)pred.Count()
                }).Aggregate((curr, accum) =>
                {
                    accum.TotalAccuracy += curr.TotalAccuracy;
                    accum.AverageAccuracy += curr.AverageAccuracy;
                    return accum;
                });
            result.TotalAccuracy /= predicted.Count();
            result.AverageAccuracy /= predicted.Count();
            return result;
        }
    }
}
