using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TrainApp
{
    internal class RecordsTransformer
    {
        private PropertyInfo[] TrainingSampleProps;
        private ITagger Tagger;

        public RecordsTransformer()
        {
            Tagger = new NameTagger();
            TrainingSampleProps = typeof(NameTrainingSample).GetProperties()
                .Where(p => p.Name != nameof(NameTrainingSample.Input))
                .ToArray();
        }
        
        public Tuple<int[][], int[][]> GetXAndY(NameTrainingSample[] samples)
        {
            return new Tuple<int[][], int[][]>(
                samples.Select(s => TagSample(s)).ToArray(),
                samples.Select(s => LabelSample(s)).ToArray());
        }

        private int[] LabelSample(NameTrainingSample sample)
        {
            return TrainingSampleProps
                .Select((prop, i) =>
                    Preprocessing.GetFormattedWords(prop.GetValue(sample)?.ToString() ?? "")
                        .Select(x => (int)typeof(NameLabel).GetEnumValues().GetValue(i)))
                .SelectMany(x => x)
                .ToArray();
        }

        private int[] TagSample(NameTrainingSample sample)
        {
            return Tagger.TagInput(Preprocessing.GetFormattedWords(sample.Input));
        }
    }
}
