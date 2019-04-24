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
            Tagger = new Tagger();
            TrainingSampleProps = typeof(TrainingSample).GetProperties()
                .Where(p => p.Name != nameof(TrainingSample.Input))
                .ToArray();
        }
        
        public Tuple<int[][], int[][]> GetXAndY(TrainingSample[] samples)
        {
            return new Tuple<int[][], int[][]>(
                samples.Select(s => TagSample(s)).ToArray(),
                samples.Select(s => LabelSample(s)).ToArray());
        }

        private int[] LabelSample(TrainingSample sample)
        {
            return TrainingSampleProps
                .Select((prop, i) =>
                    Preprocessing.GetFormattedWords(prop.GetValue(sample)?.ToString() ?? "")
                        .Select(x => (int)typeof(Label).GetEnumValues().GetValue(i)))
                .SelectMany(x => x)
                .ToArray();
        }

        private int[] TagSample(TrainingSample sample)
        {
            return Tagger.TagInput(Preprocessing.GetFormattedWords(sample.Input));
        }
    }
}
