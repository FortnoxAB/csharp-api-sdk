using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Fortnox.SDK;
using Fortnox.SDK.Entities;
using Fortnox.SDK.Exceptions;
using Fortnox.SDK.Interfaces;
using Fortnox.SDK.Search;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortnoxSDK.Tests;

[TestClass]
public class ReportedIssuesTests
{
    public FortnoxClient FortnoxClient = TestUtils.DefaultFortnoxClient;

    [TestMethod]
    public async Task Test_Issue_44() // Origins from https://github.com/FortnoxAB/csharp-api-sdk/issues/44
    {
        //Arrange
        var customerConnector = FortnoxClient.CustomerConnector;
        var tmpCustomer = await customerConnector.CreateAsync(new Customer() { Name = "TmpTestCustomer" });

        var connector = FortnoxClient.InvoiceConnector;

        var newInvoce = await connector.CreateAsync(new Invoice()
        {
            InvoiceDate = new DateTime(2019, 1, 20), //"2019-01-20",
            DueDate = new DateTime(2019, 2, 20), //"2019-02-20",
            CustomerNumber = tmpCustomer.CustomerNumber,
            InvoiceType = InvoiceType.Invoice,
            InvoiceRows = new List<InvoiceRow>()
            { //Add Empty rows
                new InvoiceRow(), //Empty Row
                new InvoiceRow(), //Empty Row
                new InvoiceRow() { AccountNumber = 0000},
                new InvoiceRow(), //Empty Row
            }
        });
    }

    [TestMethod]
    public async Task Test_issue51_fixed() //Origins from https://github.com/FortnoxAB/csharp-api-sdk/issues/51
    {
        //Arrange
        /* Assuming several (at least 5) vouchers exists */

        //Act & Assert
        var connector = FortnoxClient.VoucherConnector;
        var searchSettings = new VoucherSearch();
        searchSettings.FinancialYearID = 1;

        searchSettings.Limit = 2;
        var voucherResult = await connector.FindAsync(searchSettings);

        searchSettings.Page = 2;
        var voucherResult2 = await connector.FindAsync(searchSettings);

        searchSettings.Page = 3;
        var voucherResult3 = await connector.FindAsync(searchSettings);
    }

    [TestMethod]
    public async Task Test_issue57_fixed() // Origins from https://github.com/FortnoxAB/csharp-api-sdk/issues/57
    {
        var connector = FortnoxClient.CustomerConnector;
        var specificCustomer = await connector.CreateAsync(new Customer() { Name = "TestCustomer", OrganisationNumber = "123456789" });

        var searchSettings = new CustomerSearch();
        searchSettings.OrganisationNumber = "123456789";
        var customers = (await connector.FindAsync(searchSettings)).Entities;
        var customer = customers.FirstOrDefault(c => c.CustomerNumber == specificCustomer.CustomerNumber);
        Assert.IsNotNull(customer);

        await connector.DeleteAsync(specificCustomer.CustomerNumber);
    }

    [TestMethod]
    public async Task Test_issue50_fixed() // Origins from https://github.com/FortnoxAB/csharp-api-sdk/issues/50
    {
        var connector = FortnoxClient.CustomerConnector;
        var newCustomer = await connector.CreateAsync(new Customer() { Name = "TestCustomer", City = "Växjö", Type = CustomerType.Company });

        var updatedCustomer = await connector.UpdateAsync(new Customer() { CustomerNumber = newCustomer.CustomerNumber, City = "Stockholm" });
        Assert.AreEqual(CustomerType.Company, updatedCustomer.Type);

        await connector.DeleteAsync(newCustomer.CustomerNumber);
    }

    [TestMethod]
    public async Task Test_issue61_fixed() // Origins from https://github.com/FortnoxAB/csharp-api-sdk/issues/61
    {
        var connector = FortnoxClient.ArticleConnector;
        var newArticle = await connector.CreateAsync(new Article() { Description = "TestArticle", FreightCost = 10, OtherCost = 10, CostCalculationMethod = "MANUAL" });

        //NOTE: Server does not create the properties FreightCost, OtherCost and CostCalculationMethod
        //Assert.AreEqual("10", newArticle.FreightCost); //Always fails
        await connector.DeleteAsync(newArticle.ArticleNumber);
    }

    [TestMethod]
    public async Task Test_issue73_async_non_blockable()
    {
        var connector = FortnoxClient.CustomerConnector;
        var searchSettings = new CustomerSearch();
        searchSettings.Limit = 2;

        var watch = new Stopwatch();
        watch.Start();

        var runningTasks = new List<Task<EntityCollection<CustomerSubset>>>();
        for (var i = 0; i < 40; i++)
            runningTasks.Add(connector.FindAsync(searchSettings));

        Console.WriteLine(@"Thread free after: " + watch.ElapsedMilliseconds);
        Assert.IsTrue(watch.ElapsedMilliseconds < 1000);

        watch.Start();
        foreach (var runningTask in runningTasks)
        {
            var result = await runningTask;
            Assert.IsNotNull(result);
        }
        watch.Stop();
        Console.WriteLine(@"Total time: " + watch.ElapsedMilliseconds);
    }

    [TestMethod]
    public async Task Test_issue84_fixed() //Origins from https://github.com/FortnoxAB/csharp-api-sdk/issues/84
    {
        //Arrange
        var conn = FortnoxClient.ArchiveConnector;
        var testRootFolder = await conn.GetFolderAsync("TestArchive") ?? await conn.CreateFolderAsync("TestArchive");

        //Act
        var connector = FortnoxClient.ArchiveConnector;

        var data = Resource.fortnox_image;
        var randomFileName = TestUtils.RandomString() + "åöä.txt";

        var fortnoxFile = await connector.UploadFileAsync(randomFileName, data, testRootFolder.Name);

        //Assert
        Assert.AreEqual(randomFileName, fortnoxFile.Name);

        //Clean
        await connector.DeleteFileAsync(fortnoxFile.Id);
    }

    [TestMethod]
    public async Task Test_Issue95_fixed() //Origins from https://github.com/FortnoxAB/csharp-api-sdk/issues/95
    {
        //Arrange
        //Creates a customer with ElectronicInvoice option for deliviery type
        var connector = FortnoxClient.CustomerConnector;
        var tmpCustomer = await connector.CreateAsync(new Customer()
        {
            Name = "TestCustomer",
            DefaultDeliveryTypes = new DefaultDeliveryTypes()
            {
                Invoice = DefaultDeliveryType.ElectronicInvoice
            }
        });

        //Act && Assert
        var customer = await connector.GetAsync(tmpCustomer.CustomerNumber);
        Assert.AreEqual(DefaultDeliveryType.ElectronicInvoice, customer.DefaultDeliveryTypes.Invoice);

        //Clean
        await connector.DeleteAsync(tmpCustomer.CustomerNumber);
    }

    [TestMethod]
    public async Task Test_Issue96_fixed() // Origins from https://github.com/FortnoxAB/csharp-api-sdk/issues/96
    {
        #region Arrange
        var tmpSupplier = await FortnoxClient.SupplierConnector.CreateAsync(new Supplier() { Name = "TmpSupplier" });
        var tmpArticle = await FortnoxClient.ArticleConnector.CreateAsync(new Article() { Description = "TmpArticle", PurchasePrice = 100 });
        #endregion Arrange

        var connector = FortnoxClient.SupplierInvoiceConnector;

        var createdInvoice = await connector.CreateAsync(new SupplierInvoice()
        {
            SupplierNumber = tmpSupplier.SupplierNumber,
            Comments = "InvoiceComments",
            InvoiceDate = new DateTime(2010, 1, 1),
            DueDate = new DateTime(2010, 2, 1),
            SalesType = SalesType.Stock,
            OCR = "1234567897",
            Total = 5000,
            SupplierInvoiceRows = new List<SupplierInvoiceRow>()
            {
                new SupplierInvoiceRow() {ArticleNumber = tmpArticle.ArticleNumber, Quantity = 10, Price = 100},
                new SupplierInvoiceRow() {ArticleNumber = tmpArticle.ArticleNumber, Quantity = 20, Price = 100},
                new SupplierInvoiceRow() {ArticleNumber = tmpArticle.ArticleNumber, Quantity = 20, Price = 100}
            }
        });
        Assert.AreEqual(false, createdInvoice.Cancelled);

        var retrievedInvoice = await connector.GetAsync(createdInvoice.GivenNumber);
        Assert.AreEqual(false, retrievedInvoice.Cancelled);

        var searchSettings = new SupplierInvoiceSearch();
        searchSettings.LastModified = DateTime.Today;
        var invoiceSubsets = (await connector.FindAsync(searchSettings)).Entities;
        foreach (var supplierInvoiceSubset in invoiceSubsets)
            Assert.IsNotNull(supplierInvoiceSubset.Cancelled);

        #region Delete arranged resources

        await FortnoxClient.SupplierInvoiceConnector.CancelAsync(createdInvoice.GivenNumber);
        await FortnoxClient.SupplierConnector.DeleteAsync(tmpSupplier.SupplierNumber);
        await FortnoxClient.ArticleConnector.DeleteAsync(tmpArticle.ArticleNumber);

        #endregion Delete arranged resources
    }

    [TestMethod]
    public async Task Test_Issue98_fixed() // Origins from https://github.com/FortnoxAB/csharp-api-sdk/issues/98
    {
        #region Arrange

        var tmpCustomer = await FortnoxClient.CustomerConnector.CreateAsync(new Customer()
        { Name = "TmpCustomer", CountryCode = "SE", City = "Testopolis" });
        var tmpArticle = await FortnoxClient.ArticleConnector.CreateAsync(new Article()
        { Description = "TmpArticle", PurchasePrice = 100 });

        #endregion Arrange

        var connector = FortnoxClient.InvoiceConnector;

        var largeId = (long)2 * int.MaxValue + TestUtils.RandomInt();

        var newInvoice = new Invoice()
        {
            DocumentNumber = largeId,
            CustomerNumber = tmpCustomer.CustomerNumber,
            InvoiceDate = new DateTime(2019, 1, 20),
            DueDate = new DateTime(2019, 2, 20),
            InvoiceType = InvoiceType.CashInvoice,
            PaymentWay = PaymentWay.Cash,
            Comments = "TestInvoice",
            InvoiceRows = new List<InvoiceRow>()
            {
                new InvoiceRow() {ArticleNumber = tmpArticle.ArticleNumber, DeliveredQuantity = 10, Price = 100},
                new InvoiceRow() {ArticleNumber = tmpArticle.ArticleNumber, DeliveredQuantity = 20, Price = 100},
                new InvoiceRow() {ArticleNumber = tmpArticle.ArticleNumber, DeliveredQuantity = 15, Price = 100}
            }
        };

        var createdInvoice = await connector.CreateAsync(newInvoice);
        Assert.AreEqual(largeId, createdInvoice.DocumentNumber);

        #region Delete arranged resources

        await FortnoxClient.InvoiceConnector.CancelAsync(createdInvoice.DocumentNumber);
        await FortnoxClient.CustomerConnector.DeleteAsync(tmpCustomer.CustomerNumber);
        //FortnoxClient.ArticleConnector.Delete(tmpArticle.ArticleNumber);

        #endregion Delete arranged resources
    }

    [TestMethod]
    public async Task Test_Issue99_v1_fixed() // Origins from https://github.com/FortnoxAB/csharp-api-sdk/issues/99
    {
        #region Arrange
        var ac = FortnoxClient.ArchiveConnector;

        var data = Resource.fortnox_image;
        var randomFileName = TestUtils.RandomString() + ".txt";

        var fortnoxFile = await ac.UploadFileAsync(randomFileName, data, StaticFolders.SupplierInvoices);

        #endregion Arrange

        var archiveConnector = FortnoxClient.ArchiveConnector;
        var case1 = await archiveConnector.DownloadFileAsync(fortnoxFile.Id, IdType.Id); //no error
        await Assert.ThrowsExceptionAsync<FortnoxApiException>(
            async () => await archiveConnector.DownloadFileAsync(fortnoxFile.Id, IdType.FileId)); //has error
        await Assert.ThrowsExceptionAsync<FortnoxApiException>(
            async () => await archiveConnector.DownloadFileAsync(fortnoxFile.ArchiveFileId, IdType.Id)); //has error
        var case4 = await archiveConnector.DownloadFileAsync(fortnoxFile.ArchiveFileId, IdType.FileId); //no error

        Assert.IsNotNull(case1);
        Assert.IsNotNull(case4);

        var inboxConnector = FortnoxClient.InboxConnector;
        var case5 = await inboxConnector.DownloadFileAsync(fortnoxFile.Id, IdType.Id); //no error
        var case6 = await inboxConnector.DownloadFileAsync(fortnoxFile.Id, IdType.FileId); //no error, why?

        await Assert.ThrowsExceptionAsync<FortnoxApiException>(
            async () => await inboxConnector.DownloadFileAsync(fortnoxFile.ArchiveFileId, IdType.Id)); //has error
        var case8 = await inboxConnector.DownloadFileAsync(fortnoxFile.ArchiveFileId, IdType.FileId); //no error

        Assert.IsNotNull(case5);
        Assert.IsNotNull(case6);
        Assert.IsNotNull(case8);

        //Clean
        await archiveConnector.DeleteFileAsync(fortnoxFile.Id);
    }

    [TestMethod]
    public async Task Test_Issue99_v2_fixed() // Origins from https://github.com/FortnoxAB/csharp-api-sdk/issues/99
    {
        #region Arrange
        var ac = FortnoxClient.InboxConnector;

        var data = Resource.fortnox_image;
        var randomFileName = TestUtils.RandomString() + ".txt";

        var fortnoxFile = await ac.UploadFileAsync(randomFileName, data, StaticFolders.SupplierInvoices);

        #endregion Arrange

        var archiveConnector = FortnoxClient.ArchiveConnector;
        var case1 = await archiveConnector.DownloadFileAsync(fortnoxFile.Id, IdType.Id); //no error
        await Assert.ThrowsExceptionAsync<FortnoxApiException>(
            async () => await archiveConnector.DownloadFileAsync(fortnoxFile.Id, IdType.FileId)); //has error
        await Assert.ThrowsExceptionAsync<FortnoxApiException>(
            async () => await archiveConnector.DownloadFileAsync(fortnoxFile.ArchiveFileId, IdType.Id)); //has error
        var case4 = await archiveConnector.DownloadFileAsync(fortnoxFile.ArchiveFileId, IdType.FileId); //no error

        Assert.IsNotNull(case1);
        Assert.IsNotNull(case4);

        var inboxConnector = FortnoxClient.InboxConnector;
        var case5 = await inboxConnector.DownloadFileAsync(fortnoxFile.Id, IdType.Id); //no error
        var case6 = await inboxConnector.DownloadFileAsync(fortnoxFile.Id, IdType.FileId); //no error, why?
        await Assert.ThrowsExceptionAsync<FortnoxApiException>(
            async () => await inboxConnector.DownloadFileAsync(fortnoxFile.ArchiveFileId, IdType.Id)); //has error
        var case8 = await inboxConnector.DownloadFileAsync(fortnoxFile.ArchiveFileId, IdType.FileId); //no error

        Assert.IsNotNull(case5);
        Assert.IsNotNull(case6);
        Assert.IsNotNull(case8);

        //Clean
        await inboxConnector.DeleteFileAsync(fortnoxFile.Id);
    }

    /// <summary>
    /// Requires at least one fully paid invoice
    /// </summary>
    [TestMethod]
    public async Task Test_Issue164_InvoiceSubset_HasFinalPayDate() // Origins from https://github.com/FortnoxAB/csharp-api-sdk/issues/164
    {
        var connector = FortnoxClient.InvoiceConnector;

        var query = new InvoiceSearch()
        {
            FilterBy = Filter.Invoice.FullyPaid
        };

        var invoices = (await connector.FindAsync(query)).Entities;
        Assert.AreNotEqual(0, invoices.Count);

        foreach (var invoice in invoices)
        {
            Assert.IsNotNull(invoice.FinalPayDate);
        }
    }
}