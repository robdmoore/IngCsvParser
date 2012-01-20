using System;
using NUnit.Framework;
using XeroIngCsvParser.Constants;
using XeroIngCsvParser.Tests.Helpers;

namespace XeroIngCsvParser.Tests
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
    }
}
