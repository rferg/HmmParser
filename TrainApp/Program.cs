using System;
using System.Text;

namespace TrainApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var defaultColor = Console.ForegroundColor;
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

            while (true)
            {
                Console.WriteLine("What do you want to do?");
                Console.WriteLine(" N: train a new model");
                Console.WriteLine(" T: test an existing model");
                Console.WriteLine(" P: use an existing model to parse names in the console");
                Console.WriteLine("Enter Option:");
                ConsoleKey key = Console.ReadKey().Key;

                if (key == ConsoleKey.N)
                {
                    
                }

                if (key == ConsoleKey.T)
                {

                }

                if (key == ConsoleKey.P)
                {

                }
            }
            
        }

        static void Train(string filePath)
        {
            // todo
        }

        static void Test(string filePath, string modelPath)
        {
            // todo
        }

        static void ParseFromConsole(string modelPath)
        {
            // todo
        }
    }
}
