using System.Diagnostics;
using NUnit.Framework;
using XeroIngCsvParser.Constants;
using XeroIngCsvParser.Tests.Helpers;

namespace XeroIngCsvParser.Tests
{
    [TestFixture]
    class ConsoleApplicationShould
    {
        #region Setup

        private static CommandResult Run(string arguments)
        {
            var result = new CommandResult();

            var info = new ProcessStartInfo(TestFile.Get("XeroIngCsvParser.exe"))
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
    }


    
}
