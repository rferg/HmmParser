using Accord.Statistics.Models.Markov;
using Common;
using System;
using System.Globalization;

namespace HmmNameParser
{
    /// <summary>
    /// Provides method for parsing full name <see cref="string"/> into <see cref="Name"/> class.
    /// </summary>
    public class Parser
    {
        private HiddenMarkovModel HMM;
        private ITagger Tagger;
        private TextInfo TextInfo;

        /// <summary>
        /// Initializes <see cref="Parser"/> with default path to serialized <see cref="HiddenMarkovModel"/>.
        /// </summary>
        public Parser() : this(new Tagger(), new ModelLoader())
        {            
        }

        internal Parser(ITagger tagger, IModelLoader modelLoader)
        {
            TextInfo = new CultureInfo("en-US", false).TextInfo;
            Tagger = tagger;
            HMM = modelLoader.LoadHMM();
        }

        /// <summary>
        /// Parses <paramref name="name"/> into its component parts and returns <see cref="Name"/>.
        /// </summary>
        /// <param name="name">name <see cref="string"/> to parse</param>
        /// <returns></returns>
        public Name Parse(string name)
        {
            string[] words = Preprocessing.GetFormattedWords(name?.Trim() ?? "");
            int[] tags = Tagger.TagInput(words);
            int[] labels = HMM.Decide(tags);
            return AssignToNameFromLabels(labels, words);
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
                string word = ToTitleCase(words[i]?.Trim());

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

        private string ToTitleCase(string word)
        {
            return TextInfo.ToTitleCase(word ?? "");
        }
    }
}
