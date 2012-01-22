using System;
using IngCsvParser.Classes;
using IngCsvParser.Constants;
using NUnit.Framework;

namespace IngCsvParser.Tests
{
    [TestFixture]
    class IngCsvDescriptionShould
    {
        [Test, Sequential]
        public void Correctly_parse_description(
            [Values(
                "Pay Anyone - {DESCRIPTION} - Transfer to {PAYEE} - Receipt 999 To {ACCOUNT_NO}",
                "BPAY - {DESCRIPTION} - BPAY Bill Payment - Receipt 999  To {PAYEE}",
                "Deposit - {DESCRIPTION} - {DESCRIPTION} Deposit - Receipt 999 {PAYEE}",
                "Deposit - {PAYEE}         {DESCRIPTION} - Receipt 999",
                "Deposit - {PAYEE/DESCRIPTION} - Receipt 999",
                "Direct Debit - {PAYEE} - Direct Debit - Receipt 999  {DESCRIPTION}",
                "Transfer - To my account {PAYEE} - {DESCRIPTION} - Receipt 999",
                "Transfer - {DESCRIPTION} - Internal Transfer - Receipt 999  {PAYEE}",
                "Cash & Purchase - {PAYEE}   {SUBURB}     {STATE} - {DESCRIPTION} - Receipt 999 Date {DD/MM/YYYY} Time {H:MM} PM Card ************{LAST_4_CARD_DIGITS}",
                "Cash & Purchase - {PAYEE} - Receipt 999 ATM owner fee of ${AMOUNT} charged by {PAYEE} Date {DD/MM/YYYY} Time {H:MM} PM Card ************{LAST_4_CARD_DIGITS}",
                "Cash & Purchase - {PAYEE} - {DESCRIPTION} - Receipt 999 In {SUBURB} Date {DD/MM/YYYY} Card ************{LAST_4_CARD_DIGITS}",
                "Cash & Purchase - {PAYEE} - {DESCRIPTION} - Receipt 999 Foreign Currency Amount: {CURRENCY} {AMOUNT} In {AREA_CODE?} Date {DD/MM/YYYY} Card ************{LAST_4_CARD_DIGITS}",
                "ING DIRECT - {DESCRIPTION} - Receipt 999"
            )] string description,
            [Values(
                PaymentType.PayAnyone,
                PaymentType.BPay,
                PaymentType.Deposit,
                PaymentType.Deposit,
                PaymentType.Deposit,
                PaymentType.DirectDebit,
                PaymentType.InternalTransfer,
                PaymentType.InternalTransfer,
                PaymentType.CashAndPurchase,
                PaymentType.CashAndPurchase,
                PaymentType.CashAndPurchase,
                PaymentType.CashAndPurchase,
                PaymentType.Fee
            )] PaymentType expectedPaymentType
        )
        {
            var transaction = new Transaction { FullDetails = description };

            IngCsvDescription.Extract(transaction);

            Assert.That(transaction.PaymentType, Is.EqualTo(expectedPaymentType), description);
            Assert.That(transaction.Description, expectedPaymentType == PaymentType.CashAndPurchase && description.Contains("ATM")
                ? Is.EqualTo("ATM Withdrawal")
                : Is.EqualTo("{DESCRIPTION}").Or.EqualTo("{PAYEE/DESCRIPTION}")
            );
            Assert.That(transaction.Payee, expectedPaymentType == PaymentType.Fee
                ? Is.EqualTo("ING Direct")
                : Is.EqualTo("{PAYEE}").Or.EqualTo("{PAYEE/DESCRIPTION}"), description
            );
            Assert.That(transaction.ReferenceNumber, Is.EqualTo("999"), description);
        }

        [Test]
        public void Throw_exception_for_invalid_payment_type()
        {
            var transaction = new Transaction { FullDetails = "SomeWeirdPaymentType - Description - Receipt 999" };

            var ex = Assert.Throws<ApplicationException>(() => IngCsvDescription.Extract(transaction));

            Assert.That(ex.Message, Is.EqualTo(Errors.UnknownPaymentType + "SomeWeirdPaymentType"));
        }

        [Test]
        public void Throw_exception_for_invalid_number_of_segments_in_the_transaction_details()
        {
            var transaction = new Transaction { FullDetails = "Pay Anyone - Description" };

            var ex = Assert.Throws<ApplicationException>(() => IngCsvDescription.Extract(transaction));

            Assert.That(ex.Message, Is.EqualTo(Errors.InvalidTransaction + transaction.FullDetails));
        }
        
        [Test]
        public void Throw_exception_for_valid_transaction_but_with_no_receipt_number()
        {
            var transaction = new Transaction { FullDetails = "Pay Anyone - {DESCRIPTION} - Transfer to {PAYEE} - To {ACCOUNT_NO}" };

            var ex = Assert.Throws<ApplicationException>(() => IngCsvDescription.Extract(transaction));

            Assert.That(ex.Message, Is.EqualTo(Errors.NoReceiptNumber + transaction.FullDetails));
        }

        [Test]
        public void Throw_exception_for_valid_transaction_with_non_numeric_receipt_number()
        {
            var transaction = new Transaction { FullDetails = "Pay Anyone - {DESCRIPTION} - Transfer to {PAYEE} - Receipt XXX To {ACCOUNT_NO}" };

            var ex = Assert.Throws<ApplicationException>(() => IngCsvDescription.Extract(transaction));

            Assert.That(ex.Message, Is.EqualTo(Errors.NoReceiptNumber + transaction.FullDetails));
        }
    }
}
