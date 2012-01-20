using XeroIngCsvParser.Classes;

namespace XeroIngCsvParser
{
    public static class IngCsvDescription
    {
        public static void Extract(Transaction transaction)
        {
            var splitOut = transaction.Details.Split('-');
        }
    }
}