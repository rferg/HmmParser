﻿using CsvHelper.Configuration.Attributes;

namespace TrainApp
{
    internal class TrainingSample
    {
        [Name("Input")]
        public string Input { get; set; }

        [Name("Prefix")]
        public string Prefix { get; set; }

        [Name("FirstName")]
        public string FirstName { get; set; }

        [Name("MiddleName")]
        public string MiddleName { get; set; }

        [Name("LastName")]
        public string LastName { get; set; }

        [Name("Suffix")]
        public string Suffix { get; set; }
    }
}