using System;
using FizzWare.NBuilder;
using IngCsvParser.Classes;
using IngCsvParser.Constants;
using IngCsvParser.Tests.Helpers;
using NUnit.Framework;

namespace IngCsvParser.Tests
{
    [TestFixture]
    class IngCsvParserShould
    {
        [Test]
        public void Parse_valid_file()
        {
            var file = TestFile.Get("example-files\\valid_file.csv");
            var result = IngCsvParser.ParseFromCsv(file);
            
            Assert.That(result.Error, Is.Null);
            Assert.That(result.Records, Has.Count.EqualTo(5));
            Assert.That(result.Records[0].Date, Is.EqualTo(new DateTime(2012,1,18)));
            Assert.That(result.Records[0].Description, Is.EqualTo("Pay Anyone - Description 1 - Transfer to XXX - Receipt 10001 To 999999 999999"));
            Assert.That(result.Records[0].Debit, Is.EqualTo(-99.9));
            Assert.That(result.Records[0].Credit, Is.Null);
            Assert.That(result.Records[0].Balance, Is.EqualTo(999.99));
        }

        [Test]
        public void Give_error_for_nonexistant_file()
        {
            var result = IngCsvParser.ParseFromCsv("somefile");
            Assert.That(result.Records, Has.Count.EqualTo(0));
            Assert.That(result.Error, Is.EqualTo(string.Format(Errors.IncorrectFile, "somefile")));
        }

        [Test]
        public void Give_error_for_invalid_file()
        {
            var file = TestFile.Get("example-files\\invalid_file.csv");
            var result = IngCsvParser.ParseFromCsv(file);
            Assert.That(result.Records, Has.Count.EqualTo(0));
            Assert.That(result.Error, Is.EqualTo(string.Format(Errors.InvalidFile, file, "x is not a valid value for DateTime.")));
        }

        [Test]
        public void Convert_csv_records_to_transactions()
        {
            var csvRecords = Builder<IngCsvRecord>.CreateListOfSize(2)
                .All().With(r => r.Description = "ING DIRECT - {DESCRIPTION} - Receipt 999")
                .TheFirst(1).With(r => r.Debit = null)
                .TheNext(1).With(r => r.Credit = null)
                .Build();

            var transactions = IngCsvParser.ParseOutTransactions(csvRecords);
            
            Assert.That(transactions, Has.Count.EqualTo(csvRecords.Count));
            var transaction = transactions[0];
            Assert.That(transaction, Has.Property("Balance").EqualTo(csvRecords[0].Balance));
            Assert.That(transaction, Has.Property("Amount").EqualTo(csvRecords[0].Credit));
            Assert.That(transactions[1], Has.Property("Amount").EqualTo(csvRecords[1].Debit));
            Assert.That(transaction, Has.Property("Date").EqualTo(csvRecords[0].Date));
            Assert.That(transaction, Has.Property("FullDetails").EqualTo(csvRecords[0].Description));
        }

        [Test]
        public void Throw_exception_for_record_without_balance()
        {
            var csvRecords = Builder<IngCsvRecord>.CreateListOfSize(1)
                .All().With(r => r.Description = "ING DIRECT - {DESCRIPTION} - Receipt 999")
                .TheFirst(1).With(r => r.Balance = null)
                .Build();

            var ex = Assert.Throws<ApplicationException>(() => IngCsvParser.ParseOutTransactions(csvRecords));

            Assert.That(ex.Message, Is.EqualTo(Errors.NoBalance + csvRecords[0].Description));
        }

        [Test]
        public void Throw_exception_for_record_without_credit_or_debit()
        {
            var csvRecords = Builder<IngCsvRecord>.CreateListOfSize(1)
                .All().With(r => r.Description = "ING DIRECT - {DESCRIPTION} - Receipt 999")
                .TheFirst(1).With(r => r.Credit = null).And(r => r.Debit = null)
                .Build();

            var ex = Assert.Throws<ApplicationException>(() => IngCsvParser.ParseOutTransactions(csvRecords));

            Assert.That(ex.Message, Is.EqualTo(Errors.NoCreditOrDebit + csvRecords[0].Description));
        }
    }
}
