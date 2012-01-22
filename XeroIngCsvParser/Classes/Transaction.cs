using System;

namespace XeroIngCsvParser.Classes
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
        public string PayeeAndDescription { get { return string.Format("{0} - {1}", Payee, Description); } }
    }
}