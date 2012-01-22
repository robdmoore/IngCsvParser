using System.Diagnostics;
using System.IO;
using IngCsvParser.Constants;
using IngCsvParser.Tests.Helpers;
using NUnit.Framework;

namespace IngCsvParser.Tests
{
    [TestFixture]
    class ConsoleApplicationShould
    {
        #region Setup

        private static CommandResult Run(string arguments)
        {
            var result = new CommandResult();

            var info = new ProcessStartInfo(TestFile.Get("IngCsvParser.exe"))
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                Arguments = arguments
            };

            using(var process = Process.Start(info))
            {
                result.StdOut = process.StandardOutput.ReadToEnd();
                result.StdErr = process.StandardError.ReadToEnd();
            }

            return result;
        }

        internal class CommandResult
        {
            public string StdOut { get; set; }
            public string StdErr { get; set; }
        }
        #endregion

        [Test]
        public void Display_error_for_no_arguments_given()
        {
            var result = Run("");
            Assert.That(result.StdOut, Is.EqualTo("Press any key to exit...\r\n"));
            Assert.That(result.StdErr, Is.EqualTo(Errors.IncorrectArguments + "\r\n"));
        }

        [Test]
        public void Display_error_for_too_many_arguments_given()
        {
            var result = Run("arg1 arg2");
            Assert.That(result.StdOut, Is.EqualTo("Press any key to exit...\r\n"));
            Assert.That(result.StdErr, Is.EqualTo(Errors.IncorrectArguments + "\r\n"));
        }

        [Test]
        public void Display_error_for_uncaught_exception()
        {
            var result = Run(TestFile.Get("example-files\\no_receipt_no.csv", true));
            Assert.That(result.StdOut, Is.EqualTo("Press any key to exit...\r\n"));
            Assert.That(result.StdErr, Is.Not.Empty.And.Contains("Encountered transaction without receipt number"));
        }

        [Test]
        public void Output_valid_output_file_for_valid_input_file()
        {
            var result = Run(TestFile.Get("example-files\\valid_file.csv", true));
            Assert.That(result.StdOut, Is.EqualTo("Success!\r\nPress any key to exit...\r\n"));
            Assert.That(result.StdErr, Is.Empty);

            var expectedOutput = File.ReadAllText(TestFile.Get("example-files\\valid_output.csv"));
            var actualOutput = File.ReadAllText(TestFile.Get("out.csv"));
            Assert.That(actualOutput, Is.EqualTo(expectedOutput));
        }
    }
}
