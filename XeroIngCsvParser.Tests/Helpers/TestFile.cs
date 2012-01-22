using System.IO;
using System.Reflection;

namespace XeroIngCsvParser.Tests.Helpers
{
    public static class TestFile
    {
        public static string Get(string fileName, bool wrapWithQuotes = false)
        {
            return
                (wrapWithQuotes ? @"""" : "")
                + Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", ""))
                + "\\" + fileName
                + (wrapWithQuotes ? @"""" : "");
        }
    }
}
