using System.Collections.Generic;

namespace XeroIngCsvParser.Classes
{
    public class ParseResult
    {
        public ParseResult()
        {
            Records = new List<IngCsvRecord>();
        }
        public List<IngCsvRecord> Records { get; private set; }
        public string Error { get; set; }
    }
}