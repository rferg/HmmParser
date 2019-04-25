using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TrainApp
{
    internal class RecordsTransformer<SampleType, LabelEnum, TaggerType>
        where SampleType : Sample
        where TaggerType : ITagger, new()
    {
        private PropertyInfo[] TrainingSampleProps;
        private ITagger Tagger;

        public RecordsTransformer()
        {
            Tagger = new TaggerType();
            TrainingSampleProps = typeof(SampleType).GetProperties()
                .Where(p => p.Name != nameof(Sample.Input))
                .ToArray();
        }
        
        public Tuple<int[][], int[][]> GetXAndY(SampleType[] samples, params char[] additionalSplitChars)
        {
            return new Tuple<int[][], int[][]>(
                samples.Select(s => TagSample(s, additionalSplitChars)).ToArray(),
                samples.Select(s => LabelSample(s, additionalSplitChars)).ToArray());
        }

        private int[] LabelSample(SampleType sample, char[] splitChars)
        {
            return TrainingSampleProps
                .Select((prop, i) =>
                    Preprocessing.GetFormattedWords(prop.GetValue(sample)?.ToString() ?? "", splitChars)
                        .Select(x => (int)typeof(LabelEnum).GetEnumValues().GetValue(i)))
                .SelectMany(x => x)
                .ToArray();
        }

        private int[] TagSample(SampleType sample, char[] splitChars)
        {
            return Tagger.TagInput(Preprocessing.GetFormattedWords(sample.Input, splitChars));
        }
    }
}
