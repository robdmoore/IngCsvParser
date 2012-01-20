using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper.Configuration;

namespace XeroIngCsvParser
{
    public class IngCsvParser
    {
        public static ParseResult ParseFromCsv(string fileName)
        {
            var result = new ParseResult();

            try
            {
                var csv = new CsvHelper.CsvHelper(File.OpenRead(fileName));
                var records = csv.Reader.GetRecords<IngCsvRecord>();
                result.Records.AddRange(records);
            }
            catch(FileNotFoundException)
            {
                result.Error = string.Format(Errors.IncorrectFile, fileName);
            }
            catch (Exception e)
            {
                result.Error = string.Format("Invalid ING CSV file '{0}' given; {1}.", fileName, e.Message);
            }
            
            return result;
        }
    }

    public class ParseResult
    {
        public ParseResult()
        {
            Records = new List<IngCsvRecord>();
        }
        public List<IngCsvRecord> Records { get; private set; }
        public string Error { get; set; }
    }

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
