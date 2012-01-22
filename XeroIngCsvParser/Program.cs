using System;
using System.IO;
using XeroIngCsvParser.Constants;

namespace XeroIngCsvParser
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine(Errors.IncorrectArguments);
            }
            else
            {
                var result = IngCsvParser.ParseFromCsv(args[0]);
                if (!string.IsNullOrEmpty(result.Error))
                {
                    Console.Error.WriteLine(result.Error);
                }
                else if (result.Records.Count == 0)
                {
                    Console.Error.WriteLine(Errors.NoRecords);
                }
                else
                {
                    try
                    {
                        var fullyParsed = IngCsvParser.ParseOutTransactions(result.Records);
                        var csv = new CsvHelper.CsvHelper(File.OpenWrite("out.csv"));
                        csv.Writer.WriteRecords(fullyParsed);
                    }
                    catch(Exception e)
                    {
                        Console.Error.WriteLine(e.ToString());
                    }
                }
            }
            
            Console.WriteLine("Press any key to exit...");
            Console.Read();
        }
    }
}
