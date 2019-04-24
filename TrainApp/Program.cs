using Accord.IO;
using Accord.Statistics.Models.Markov;
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
                ConsoleKey key = PromptForMainOption();

                try
                {
                    if (key == ConsoleKey.N)
                    {
                        Train();
                    }
                    else if (key == ConsoleKey.T)
                    {
                        string modelPath =
                            GetFilePath(REQUIRED_MODEL_EXTENSION, "Please enter path to your model file:");
                        HiddenMarkovModel hmm = Serializer.Load<HiddenMarkovModel>(modelPath);
                        Test(hmm);
                    }
                    else if (key == ConsoleKey.P)
                    {
                        string modelPath =
                            GetFilePath(REQUIRED_MODEL_EXTENSION, "Please enter path to your model file:");
                        HiddenMarkovModel hmm = Serializer.Load<HiddenMarkovModel>(modelPath);
                        ParseFromConsole(hmm);
                    }
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
            Console.WriteLine("What do you want to do?");
            Console.WriteLine(" N: train a new model");
            Console.WriteLine(" T: test an existing model");
            Console.WriteLine(" P: use an existing model to parse names in the console");
            Console.WriteLine("Enter Option:");
            return Console.ReadKey().Key;
        }

        static void Train()
        {
            string filePath = GetFilePath(".csv", "Please enter path to training data file:");
            Tuple<int[][], int[][]> xAndY = GetTransformedData(filePath);
            Console.WriteLine("Training model...");
            ModelTrainer trainer = new ModelTrainer();
            HiddenMarkovModel hmm = trainer.TrainModel(xAndY.Item1, xAndY.Item2);
            Console.WriteLine("Model trained.");

            Console.WriteLine("Do you want to (T)est this model or (S)ave it right away?");
            ConsoleKey key = Console.ReadKey().Key;
            if (key == ConsoleKey.T)
            {
                Test(hmm);
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

        static void Test(HiddenMarkovModel hmm)
        {
            string filePath = GetFilePath(".csv", "Please enter path to test data file:");
            Tuple<int[][], int[][]> xAndY = GetTransformedData(filePath);
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

        static Tuple<int[][], int[][]> GetTransformedData(string filePath)
        {
            RecordsLoader loader = new RecordsLoader(filePath);
            RecordsTransformer transformer = new RecordsTransformer();
            Console.WriteLine("Loading and transforming records...");
            return transformer.GetXAndY(loader.Load());
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
