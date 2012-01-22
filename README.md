ING Direct Australia CSV Export Parser
======================================

This project builds an executable file that can be used to parse the result of a CSV export from the [Internet Banking Portal](http://www.ingdirect.com.au) that ING Direct Australia provide.

The CSV records are parsed such that the Payee and Description of the different records are separated out into separate fields. This means you can then use the CSV file to more easily analyse your banking transactions either manually or via tools such as [Xero](http://www.xero.com).

Motivation
----------

I created this project because I wanted to use [Xero](http://www.xero.com) as my personal financial management / tracking solution, but I couldn't because it doesn't support automatic categorisation of the excessively verbose output in the ING Direct CSV files.

Example
-------

The program will take a file such as:

    Date, Description, Debit, Credit, Balance
    18/01/2012,"Pay Anyone - Description 1 - Transfer to XXX - Receipt 10001 To 999999 999999",-99.90,, 999.99
    19/01/2012,"Cash & Purchase - Some Place - EFTPOS Purchase - Receipt 10000 Date 20/01/2012 Time 11:58 AM Card ************9999",-999.30,, 9999.99
    20/01/2012,"Pay Anyone - Description 2 - Transfer to XXX - Receipt 9999 To 999999 999998",-999.67,, 9999.89
    21/01/2012,"Transfer - To my account 9999 - Internal Transfer - Receipt 9998",,90.00, 127.56
    22/01/2012,"Transfer - To my account 99999 - Internal Transfer - Receipt 9997",-100.00,, 90.56 

And turn it into a file like this:

    Date,Payee,Description,Amount,Balance,ReferenceNumber,FullDetails,PaymentType,PayeeAndDescription
    18/01/2012,XXX,Description 1,-99.9,999.99,10001,Pay Anyone - Description 1 - Transfer to XXX - Receipt 10001 To 999999 999999,PayAnyone,XXX - Description 1
    19/01/2012,Some Place,EFTPOS Purchase,-999.3,9999.99,10000,Cash & Purchase - Some Place - EFTPOS Purchase - Receipt 10000 Date 20/01/2012 Time 11:58 AM Card ************9999,CashAndPurchase,Some Place - EFTPOS Purchase
    20/01/2012,XXX,Description 2,-999.67,9999.89,9999,Pay Anyone - Description 2 - Transfer to XXX - Receipt 9999 To 999999 999998,PayAnyone,XXX - Description 2
    21/01/2012,9999,Internal Transfer,90,127.56,9998,Transfer - To my account 9999 - Internal Transfer - Receipt 9998,InternalTransfer,9999 - Internal Transfer
    22/01/2012,99999,Internal Transfer,-100,90.56,9997,Transfer - To my account 99999 - Internal Transfer - Receipt 9997,InternalTransfer,99999 - Internal Transfer

Usage
-----

 1. (Optionally) Download the source and build the executable using Visual Studio 2010 and .NET 4
 2. Get the IngCsvParser.exe file (in bin/Debug, or bin/Release) and the CsvHelper.dll file (in the same folder) and put them on your computer somewhere
 3. Drag your .csv file exported from the ING Direct Internet Banking portal onto the IngCsvParser.exe file
 4. A new file should be created called out.csv that contains the result

Usage with Xero
---------------

 1. Follow the steps above under "Usage" to create a parsed out.csv file
 2. Log into Xero and navigate to the Dashboard
 3. Click on the "Accounts" menu item and select the account you want to import transactions into
 4. Click on the "Account options" button at the top right of the graph and select "Import transactions"
 5. Click Browse and find the out.csv file, select it and then click the "Import" button
 6. Match up the following (ones marked with (\*) are important, the others are up to your personal preference, but seemed to work well for me):
   * Date -> Date (\*)
   * Payee -> Select... [i.e. blank]
   * Description -> Reference
   * Amount -> Amount (\*)
   * Balance -> Select... [i.e. blank]
   * ReferenceNumber -> Particulars
   * FullDetails -> Other
   * PaymentType -> Labels [this is nice because it allows you to use labels to distinguish between different types of transactions]
   * PayeeAndDescription -> Payee (\*)

Errors, Suggestions or Modifications
------------------------------------

Feel free to add an Issue in the Issues area, or alternatively send through a pull request and I'll happily look at either, but my speed of response will depend on how busy I am. Alternatively, feel free to fork the code and make your own version - I have released the source with an MIT license.
