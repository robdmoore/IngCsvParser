using System;
using CsvHelper.Configuration;

namespace XeroIngCsvParser.Classes
{
    public class IngCsvRecord
    {
        [CsvField( Index = 0 )]
        public DateTime Date { get; set; }
        [CsvField(Index = 1)]
        public string Description { get; set; }
        [CsvField(Index = 2)]
        public double? Debit { get; set; }
        [CsvField(Index = 3)]
        public double? Credit { get; set; }
        [CsvField(Index = 4)]
        public double? Balance { get; set; }
    }
}