using System;
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
                    var fullyParsed = IngCsvParser.ParseOutTransactions(result.Records);
                }
            }
            
            Console.WriteLine("Press any key to exit...");
            Console.Read();
        }
    }
}
