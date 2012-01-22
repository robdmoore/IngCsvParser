using IngCsvParser.Classes;
using NUnit.Framework;

namespace IngCsvParser.Tests.classes
{
    [TestFixture]
    class TransactionShould
    {
        [Test]
        public void Combine_payee_and_description_for_non_direct_debit_transactions(
            [Values(PaymentType.PayAnyone, PaymentType.InternalTransfer, PaymentType.Fee, PaymentType.Deposit, PaymentType.CashAndPurchase, PaymentType.BPay)] PaymentType paymentType
        )
        {
            var t = new Transaction { Payee = "Payee", Description = "Description", PaymentType = paymentType};
            Assert.That(t.PayeeAndDescription, Is.EqualTo("Payee - Description"));
        }

        [Test]
        public void Only_use_payee_for_direct_debit()
        {
            var t = new Transaction { Payee = "Payee", Description = "Some Unique Payment Id", PaymentType = PaymentType.DirectDebit };
            Assert.That(t.PayeeAndDescription, Is.EqualTo("Payee"));
        }
    }
}
