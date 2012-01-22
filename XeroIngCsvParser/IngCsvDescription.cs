using System;
using System.Linq;
using System.Text.RegularExpressions;
using XeroIngCsvParser.Classes;

namespace XeroIngCsvParser
{
    public static class IngCsvDescription
    {
        public static Regex ReceiptNumberAtEnd = new Regex(@"Receipt\s*(\d+)\s*$");
        public static Regex ReceiptNumber = new Regex(@"Receipt\s*(\d+)\s*");
        public static Regex AfterReceiptNumber = new Regex(@"Receipt\s*\d+\s*(.+)$");

        public static void Extract(Transaction transaction)
        {
            var splitOut = transaction.FullDetails.Split('-');
            // Payment Type
            try
            {
                transaction.PaymentType = (PaymentType) StringEnum.Parse(typeof (PaymentType), splitOut[0].Trim());
            }
            catch(Exception e)
            {
                throw new ApplicationException("Encountered unknown payment type: " + splitOut[0].Trim(), e);
            }
            try
            {
                // Payee and Description
                switch (transaction.PaymentType)
                {
                    // Pay Anyone - {DESCRIPTION} - Transfer to {NAME} - Receipt {RECEIPT_NO} To {ACCOUNT_NO}
                    case PaymentType.PayAnyone:
                        transaction.Description = splitOut[1].Trim();
                        transaction.Payee = splitOut[2].Trim().Substring("Transfer to ".Length);
                        break;
                    // BPAY - {DESCRIPTION} - BPAY Bill Payment - Receipt {RECEIPT_NO}  To {PAYEE} {PAYEE_NO}
                    case PaymentType.BPay:
                        transaction.Description = splitOut[1].Trim();
                        transaction.Payee = splitOut[3].Trim().Substring(splitOut[3].IndexOf("To") + 2);
                        break;
                    // Deposit - {DESCRIPTION} - {DESCRIPTION} Deposit - Receipt {RECEIPT_NO} {PAYEE}
                    // Deposit - {PAYEE}         {DESCRIPTION} - Receipt {RECEIPT_NO}
                    // Deposit - {PAYEE} {DESCRIPTION} - Receipt {RECEIPT_NO}
                    case PaymentType.Deposit:
                        if (ReceiptNumberAtEnd.Match(transaction.FullDetails).Success)
                        {
                            transaction.Payee = splitOut[1].Trim();
                            transaction.Description = splitOut[1].Trim();
                            var indexOfMultipleSpaces = transaction.Payee.Trim().IndexOf("  ");
                            if (indexOfMultipleSpaces != -1)
                            {
                                transaction.Payee = transaction.Payee.Substring(0, indexOfMultipleSpaces + 1).Trim();
                                transaction.Description = transaction.Description.Substring(indexOfMultipleSpaces).Trim();
                            }
                        }
                        else
                        {
                            transaction.Description = splitOut[1].Trim();
                            transaction.Payee = AfterReceiptNumber.Match(transaction.FullDetails).Groups[1].Value;
                        }
                        break;
                    // Direct Debit - {PAYEE} - Direct Debit - Receipt {RECEIPT_NO}  {DESCRIPTION}
                    case PaymentType.DirectDebit:
                        transaction.Payee = splitOut[1].Trim();
                        transaction.Description = AfterReceiptNumber.Match(transaction.FullDetails).Groups[1].Value;
                        break;
                    // Transfer - To my account {ACCOUNT_NO} - Internal Transfer - Receipt {RECEIPT_NO}
                    // Transfer - {DESCRIPTION} - Internal Transfer - Receipt {RECEIPT_NO}  {ACCOUNT_NAME} {ACCOUNT_NO}
                    case PaymentType.InternalTransfer:
                        if (splitOut[1].Trim().StartsWith("To my account"))
                        {
                            transaction.Payee = splitOut[1].Trim().Substring("To my account".Length).Trim();
                            transaction.Description = splitOut[2].Trim();
                        }
                        else
                        {
                            transaction.Description = splitOut[1].Trim();
                            transaction.Payee = AfterReceiptNumber.Match(transaction.FullDetails).Groups[1].Value;
                        }
                        break;
                    // Cash & Purchase - {PAYEE}   {SUBURB}     {STATE} - EFTPOS Purchase - Receipt {RECEIPT_NO} Date {DD/MM/YYYY} Time {H:MM} PM Card ************{LAST_4_CARD_DIGITS}
                    // Cash & Purchase - {PAYEE} - Receipt {RECEIPT_NO} ATM owner fee of ${AMOUNT} charged by {PAYEE} Date {DD/MM/YYYY} Time {H:MM} PM Card ************{LAST_4_CARD_DIGITS}
                    // Cash & Purchase - {PAYEE} - Visa Purchase - Receipt {RECEIPT_NO} In {SUBURB} Date {DD/MM/YYYY} Card ************{LAST_4_CARD_DIGITS}
                    // Cash & Purchase - {PAYEE} - Visa Purchase - Receipt {RECEIPT_NO} Foreign Currency Amount: {CURRENCY} {AMOUNT} In {AREA_CODE?} Date {DD/MM/YYYY} Card ************{LAST_4_CARD_DIGITS}
                    case PaymentType.CashAndPurchase:
                        transaction.Payee = splitOut[1].Trim();
                        if (transaction.Payee.Contains("  "))
                            transaction.Payee = transaction.Payee.Substring(0, transaction.Payee.IndexOf("  "));
                        transaction.Description = transaction.FullDetails.Contains("ATM") ? "ATM Withdrawal" : splitOut[2].Trim();
                        break;
                    // ING DIRECT - {DESCRIPTION} - Receipt {RECEIPT_NO}
                    case PaymentType.Fee:
                        transaction.Payee = "ING Direct";
                        transaction.Description = splitOut[1].Trim();
                        break;
                }
            }
            catch(Exception e)
            {
                throw new ApplicationException("Encountered invalid transaction: " + transaction.FullDetails, e);
            }
            // Receipt
            var receiptMatch = ReceiptNumber.Match(transaction.FullDetails);
            if (receiptMatch.Success)
                transaction.ReferenceNumber = receiptMatch.Groups[1].Value;
            else
                throw new ApplicationException("Encountered transaction without receipt number: " + transaction.FullDetails);
        }
    }
}