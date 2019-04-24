using Accord.Statistics.Models.Markov;
using Common;
using System;
using System.Globalization;
using System.Linq;

namespace HmmNameParser
{
    /// <summary>
    /// Provides method for parsing full name <see cref="string"/> into <see cref="Name"/> class.
    /// </summary>
    public class Parser
    {
        private IHiddenMarkovModelWrapper HMM;
        private ITagger Tagger;        
        private INameFormatter Formatter;
        private IIndividualChecker IndividualChecker;

        /// <summary>
        /// Initializes <see cref="Parser"/>.
        /// </summary>
        public Parser() : this(new Tagger(), new ModelLoader(), new NameFormatter(), new IndividualChecker())
        {            
        }

        internal Parser(
            ITagger tagger,
            IModelLoader modelLoader,
            INameFormatter formatter,
            IIndividualChecker individualChecker)
        {
            Tagger = tagger;            
            HMM = modelLoader.LoadHMM();
            Formatter = formatter;
            IndividualChecker = individualChecker;
        }

        /// <summary>
        /// Parses <paramref name="name"/> into its component parts and returns <see cref="Name"/>.
        /// </summary>
        /// <param name="name">name <see cref="string"/> to parse</param>
        /// <returns></returns>
        public Name Parse(string name)
        {
            string[] words = Preprocessing.GetFormattedWords(name?.Trim() ?? "");
            int[] labels = new int[0];
            if (IndividualChecker.IsIndividual(words))
            {
                int[] tags = Tagger.TagInput(words);
                labels = HMM.Decide(tags);
            }
            else
            {
                // if non-individual, just assign everything to LastName
                labels = words.Select(w => (int)Label.LastName).ToArray();
            }
            Name parsedName = AssignToNameFromLabels(labels, words);
            return Formatter.Format(parsedName);
        }

        private Name AssignToNameFromLabels(int[] labels, string[] words)
        {
            if (labels.Length != words.Length)
            {
                throw new Exception("Number of labels does not match number of words.");
            }

            Name name = new Name();
            for (int i = 0; i < labels.Length; i++)
            {
                Label label = (Label)labels[i];
                string word = words[i]?.Trim();

                switch (label)
                {
                    case Label.Prefix:
                        name.Prefix += string.IsNullOrEmpty(name.Prefix) ? word : " " + word;
                        break;
                    case Label.FirstName:
                        name.FirstName += string.IsNullOrEmpty(name.FirstName) ? word : " " + word;
                        break;
                    case Label.MiddleName:
                        name.MiddleName += string.IsNullOrEmpty(name.MiddleName) ? word : " " + word;
                        break;
                    case Label.LastName:
                        name.LastName += string.IsNullOrEmpty(name.LastName) ? word : " " + word;
                        break;
                    case Label.Suffix:
                        name.Suffix += string.IsNullOrEmpty(name.Suffix) ? word : " " + word;
                        break;
                    default:
                        throw new Exception($"Label {label} for '{word}' is invalid.");
                }
            }

            return name;
        }
    }
}
