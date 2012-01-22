using System;
using System.Collections.Generic;
using System.IO;
using XeroIngCsvParser.Classes;
using XeroIngCsvParser.Constants;

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
                result.Error = string.Format(Errors.InvalidFile, fileName, e.Message);
            }
            
            return result;
        }

        public static List<Transaction> ParseOutTransactions(IList<IngCsvRecord> result)
        {
            var transactions = new List<Transaction>();
            foreach (var record in result)
            {
                if (!record.Balance.HasValue)
                {
                    throw new Exception("Encountered record with no balance: " + record.Description);
                }
                if (!record.Credit.HasValue && !record.Debit.HasValue)
                {
                    throw new Exception("Encountered record with no credit or debit: " + record.Description);
                }
                var transaction = new Transaction
                {
                    Balance = record.Balance.Value,
                    Amount = record.Credit != null ? record.Credit.Value : record.Debit.Value,
                    Date = record.Date,
                    FullDetails = record.Description,
                };
                IngCsvDescription.Extract(transaction);
                transactions.Add(transaction);
            }

            return transactions;
        }
    }
}
