using Accord.IO;
using Accord.Statistics.Models.Markov;
using Common;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace TrainApp
{

    class Program
    {
        const string REQUIRED_MODEL_EXTENSION = ".bin";
        static void Main(string[] args)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            ShowTitle(defaultColor);
            
            while (true)
            {
                Console.WriteLine("What will this model be parsing?");
                Console.WriteLine(" A: Addresses");
                Console.WriteLine(" N: Names");
                ParseTarget target = ParseTarget.Name;
                ConsoleKey targetKey = Console.ReadKey().Key;
                if (targetKey == ConsoleKey.A)
                {
                    target = ParseTarget.Address;
                }
                if (targetKey == ConsoleKey.N)
                {
                    target = ParseTarget.Name;
                }
                Console.WriteLine($"Parse Target: {target}");

                ConsoleKey key = PromptForMainOption();

                try
                {
                    if (key == ConsoleKey.N)
                    {
                        Train(target);
                    }
                    else if (key == ConsoleKey.E)
                    {
                        string modelPath =
                            GetFilePath(REQUIRED_MODEL_EXTENSION, "Please enter path to your model file:");
                        HiddenMarkovModel hmm = Serializer.Load<HiddenMarkovModel>(modelPath);
                        Test(hmm, target);
                    }
                    //else if (key == ConsoleKey.P)
                    //{
                    //    string modelPath =
                    //        GetFilePath(REQUIRED_MODEL_EXTENSION, "Please enter path to your model file:");
                    //    HiddenMarkovModel hmm = Serializer.Load<HiddenMarkovModel>(modelPath);
                    //    ParseFromConsole(hmm);
                    //}
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine();
                        Console.WriteLine($"{key} is not one of the options.");
                        Console.ForegroundColor = defaultColor;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("An error occurred!");
                    Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
                    Trace.WriteLine(ex.StackTrace);
                    Console.ForegroundColor = defaultColor;
                }

            }
            
        }

        static void ShowTitle(ConsoleColor defaultColor)
        {
            Console.Title = "HmmNameParser";
            Console.ForegroundColor = ConsoleColor.Green;
            string border = "####################################";
            Console.WriteLine(border);
            Console.WriteLine(border.Replace('#', ' '));
            Console.WriteLine(" HmmNameParser -- The Model Trainer");
            Console.WriteLine(border.Replace('#', ' '));
            Console.WriteLine(border);
            Console.Write(Environment.NewLine);
            Console.ForegroundColor = defaultColor;
        }

        static ConsoleKey PromptForMainOption()
        {
            Console.WriteLine();
            Console.WriteLine("What do you want to do?");
            Console.WriteLine(" N: train a new model");
            Console.WriteLine(" E: test an existing model");
            //Console.WriteLine(" P: use an existing model to parse names in the console");
            Console.WriteLine("Enter Option:");
            return Console.ReadKey().Key;
        }

        static void Train(ParseTarget target)
        {
            string filePath = GetFilePath(".csv", "Please enter path to training data file:");
            Tuple<int[][], int[][]> xAndY = GetTransformedData(filePath, target);

            Console.WriteLine("Training model...");
            HiddenMarkovModel hmm = null;
            if (target == ParseTarget.Name)
            {
                ModelTrainer<NameTag, NameLabel> trainer = new ModelTrainer<NameTag, NameLabel>();
                hmm = trainer.TrainModel(xAndY.Item1, xAndY.Item2);
            }

            if (target == ParseTarget.Address)
            {
                ModelTrainer<AddressTag, AddressLabel> trainer = new ModelTrainer<AddressTag, AddressLabel>();
                hmm = trainer.TrainModel(xAndY.Item1, xAndY.Item2);
            }

            Console.WriteLine("Model trained.");

            Console.WriteLine("Do you want to (T)est this model or (S)ave it right away?");
            ConsoleKey key = Console.ReadKey().Key;
            if (key == ConsoleKey.T)
            {
                Test(hmm, target);
                Console.WriteLine("Save this model (Y/N)?");
                ConsoleKey saveKey = Console.ReadKey().Key;
                if (saveKey == ConsoleKey.Y)
                {
                    SaveModel(hmm);
                }
            }

            if (key == ConsoleKey.S)
            {
                SaveModel(hmm);
            }
        }

        static void Test(HiddenMarkovModel hmm, ParseTarget target)
        {
            string filePath = GetFilePath(".csv", "Please enter path to test data file:");
            Tuple<int[][], int[][]> xAndY = GetTransformedData(filePath, target);
            Console.WriteLine("Testing model...");
            ModelTester tester = new ModelTester(hmm);
            HMMTestResult results = tester.Test(xAndY);
            Console.WriteLine("Results:");
            Console.WriteLine($"    Total Accuracy: {results.TotalAccuracy:0.###}");
            Console.WriteLine($"    Average Accuracy: {results.AverageAccuracy:0.###}");
        }

        static void ParseFromConsole(HiddenMarkovModel hmm)
        {
            // TODO
        }

        static Tuple<int[][], int[][]> GetTransformedData(string filePath, ParseTarget target)
        {
            Tuple<int[][], int[][]> result = null;
            Console.WriteLine("Loading and transforming records...");
            if (target == ParseTarget.Address)
            {
                RecordsLoader<AddressTrainingSample> loader = new RecordsLoader<AddressTrainingSample>(filePath);
                RecordsTransformer<AddressTrainingSample, AddressLabel, AddressTagger> transformer =
                    new RecordsTransformer<AddressTrainingSample, AddressLabel, AddressTagger>();
                result = transformer.GetXAndY(loader.Load(), '-');
            }

            if (target == ParseTarget.Name)
            {
                RecordsLoader<NameTrainingSample> loader = new RecordsLoader<NameTrainingSample>(filePath);
                RecordsTransformer<NameTrainingSample, NameLabel, NameTagger> transformer =
                    new RecordsTransformer<NameTrainingSample, NameLabel, NameTagger>();
                result = transformer.GetXAndY(loader.Load());
            }

            return result;
        }

        static void SaveModel(HiddenMarkovModel hmm)
        {
            string path = "";
            while (string.IsNullOrEmpty(path))
            {
                Console.WriteLine();
                Console.WriteLine($"Enter the {REQUIRED_MODEL_EXTENSION} path where you want to save this model:");
                path = Console.ReadLine()?.Trim().Replace("\"", "");
                if (Path.GetExtension(path).ToLower() != REQUIRED_MODEL_EXTENSION)
                {
                    path = "";
                    Console.WriteLine($"Please enter a {REQUIRED_MODEL_EXTENSION} file name.");
                }
            }

            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            Serializer.Save(hmm, path);
            Console.WriteLine($"Model saved at {path}.");
        }

        static string GetFilePath(string extension, string message)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine(message);
                string path = Console.ReadLine()?.Trim().Replace("\"", "");

                if (Path.GetExtension(path).ToLower() != extension)
                {
                    Console.WriteLine($"Please enter the path to a {extension} file.");
                    continue;
                }

                if (!File.Exists(path))
                {
                    Console.WriteLine($"Could not find a file at {path}.");
                    continue;
                }

                return path;
            }
        }
    }
}
