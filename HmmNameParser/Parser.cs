using Accord.IO;
using Accord.Statistics.Models.Markov;
using Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HmmNameParser
{
    /// <summary>
    /// Provides method for parsing full name <see cref="string"/> into <see cref="Name"/> class.
    /// </summary>
    public class Parser
    {
        private HiddenMarkovModel HMM { get; set; }
        private Tagger Tagger { get; set; }
        private TextInfo TextInfo = new CultureInfo("en-US", false).TextInfo;

        /// <summary>
        /// Initializes <see cref="Parser"/> with default path to serialized <see cref="HiddenMarkovModel"/>.
        /// </summary>
        public Parser()
        {
            HMM = LoadHMM(Config.DEFAULT_MODEL_PATH);
            Tagger = new Tagger(
                Config.PREFIX_REF, Config.GIVEN_NAME_REF, Config.SURNAME_REF, Config.SUFFIX_REF);
        }

        /// <summary>
        /// Initializes <see cref="Parser"/> with given path to serialized <see cref="HiddenMarkovModel"/>.
        /// </summary>
        /// <param name="modelPath">The path to serialized <see cref="HiddenMarkovModel"/></param>
        public Parser(string modelPath)
        {
            HMM = LoadHMM(modelPath);
            Tagger = new Tagger(
                Config.PREFIX_REF, Config.GIVEN_NAME_REF, Config.SURNAME_REF, Config.SUFFIX_REF);
        }

        /// <summary>
        /// Parses <paramref name="name"/> into its component parts and returns <see cref="Name"/>.
        /// </summary>
        /// <param name="name">name <see cref="string"/> to parse</param>
        /// <returns></returns>
        public Name Parse(string name)
        {
            string[] words = Preprocessing.GetFormattedWords(name?.Trim() ?? "");
            int[] tags = Tagger.TagInputAsInt(words);
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

        private HiddenMarkovModel LoadHMM(string modelPath)
        {
            if (!File.Exists(modelPath))
            {
                throw new FileNotFoundException($"Couldn't find model file at {modelPath}.");
            }
            return Serializer.Load<HiddenMarkovModel>(modelPath);
        }
    }
}
