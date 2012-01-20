using System;

namespace XeroIngCsvParser.Classes
{
    public class Transaction
    {
        public DateTime Date { get; set; }
        public string Payee { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }
        public string Reference { get; set; }
        public string Details { get; set; }
        public PaymentType PaymentType { get; set; }
    }
}