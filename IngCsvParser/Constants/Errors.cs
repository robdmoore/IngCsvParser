namespace IngCsvParser.Constants
{
    public static class Errors
    {
        public const string NoReceiptNumber = "Encountered transaction without receipt number: ";
        public const string InvalidTransaction = "Encountered invalid transaction: ";
        public const string UnknownPaymentType = "Encountered unknown payment type: ";
        public const string NoBalance = "Encountered record with no balance: ";
        public const string NoCreditOrDebit = "Encountered record with no credit or debit: ";
        public const string NoRecords = "There were no records in the supplied file.";
        public const string InvalidFile = "Invalid ING CSV file '{0}' given; {1}";
        public const string IncorrectFile = "Invalid file '{0}' given.";
        public const string IncorrectArguments = "Invalid parameters given; expected a single argument - the filename of a .csv file.";
    }
}
