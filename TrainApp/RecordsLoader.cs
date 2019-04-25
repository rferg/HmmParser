using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TrainApp
{
    internal class RecordsLoader<SampleType> where SampleType : Sample
    {
        private string FilePath;
        public RecordsLoader(string filePath)
        {
            if (Path.GetExtension(filePath).ToLower() != ".csv")
            {
                throw new ArgumentException($"{filePath} is not a .csv file.", "filePath");
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"No file found at {filePath}.");
            }

            FilePath = filePath;
        }

        public SampleType[] Load()
        {
            SampleType[] records = new SampleType[0];
            using (StreamReader reader = new StreamReader(FilePath))
            using (CsvReader csv = new CsvReader(reader))
            {
                records = csv.GetRecords<SampleType>().ToArray();
            }
            return records;
        }
    }
}
