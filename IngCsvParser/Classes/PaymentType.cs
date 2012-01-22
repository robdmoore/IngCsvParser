using System.ComponentModel;

namespace IngCsvParser.Classes
{
    public enum PaymentType
    {
        [Description("Pay Anyone")]
        PayAnyone,
        [Description("Cash & Purchase")]
        CashAndPurchase,
        [Description("Transfer")]
        InternalTransfer,
        [Description("Direct Debit")]
        DirectDebit,
        [Description("Deposit")]
        Deposit,
        [Description("BPAY")]
        BPay,
        [Description("ING DIRECT")]
        Fee
    }
}