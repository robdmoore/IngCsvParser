using System;

namespace IngCsvParser.Classes
{
    public class Transaction
    {
        public DateTime Date { get; set; }
        public string Payee { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }
        public string ReferenceNumber { get; set; }
        public string FullDetails { get; set; }
        public PaymentType PaymentType { get; set; }
        // Most Direct Debit transactions seem to have the description as a unique reference number so just use Payee for them
        public string PayeeAndDescription { get { return PaymentType == PaymentType.DirectDebit ? Payee : string.Format("{0} - {1}", Payee, Description); } }
    }
}