using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fortnox.SDK;
using Fortnox.SDK.Entities;
using Fortnox.SDK.Search;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortnoxSDK.Tests.ConnectorTests;

[TestClass]
public class SupplierInvoiceTests
{
    public FortnoxClient FortnoxClient = TestUtils.DefaultFortnoxClient;

    [TestMethod]
    public async Task Test_SupplierInvoice_CRUD()
    {
        #region Arrange
        var tmpSupplier = await FortnoxClient.SupplierConnector.CreateAsync(new Supplier() { Name = "TmpSupplier" });
        var tmpArticle = await FortnoxClient.ArticleConnector.CreateAsync(new Article() { Description = "TmpArticle", PurchasePrice = 100 });
        #endregion Arrange

        var connector = FortnoxClient.SupplierInvoiceConnector;

        #region CREATE
        var newSupplierInvoice = new SupplierInvoice()
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
                new SupplierInvoiceRow(){ ArticleNumber = tmpArticle.ArticleNumber, Quantity = 10, Price = 100},
                new SupplierInvoiceRow(){ ArticleNumber = tmpArticle.ArticleNumber, Quantity = 20, Price = 100},
                new SupplierInvoiceRow(){ ArticleNumber = tmpArticle.ArticleNumber, Quantity = 20, Price = 100}
            }
        };

        var createdSupplierInvoice = await connector.CreateAsync(newSupplierInvoice);
        Assert.AreEqual("InvoiceComments", createdSupplierInvoice.Comments);
        Assert.AreEqual("TmpSupplier", createdSupplierInvoice.SupplierName);
        Assert.AreEqual(3 + 1, createdSupplierInvoice.SupplierInvoiceRows.Count);
        //3 + 1 => A row "Leverant�rsskulder" is created by default

        #endregion CREATE

        #region UPDATE

        createdSupplierInvoice.Comments = "UpdatedInvoiceComments";

        var updatedSupplierInvoice = await connector.UpdateAsync(createdSupplierInvoice);
        Assert.AreEqual("UpdatedInvoiceComments", updatedSupplierInvoice.Comments);

        #endregion UPDATE

        #region READ / GET

        var retrievedSupplierInvoice = await connector.GetAsync(createdSupplierInvoice.GivenNumber);
        Assert.AreEqual("UpdatedInvoiceComments", retrievedSupplierInvoice.Comments);

        #endregion READ / GET

        #region DELETE
        //Not supported
        await connector.CancelAsync(createdSupplierInvoice.GivenNumber);
        var cancelledInvoice = await connector.GetAsync(createdSupplierInvoice.GivenNumber);
        Assert.AreEqual(true, cancelledInvoice.Cancelled);
        #endregion DELETE

        #region Delete arranged resources
        await FortnoxClient.SupplierConnector.DeleteAsync(tmpSupplier.SupplierNumber);
        await FortnoxClient.ArticleConnector.DeleteAsync(tmpArticle.ArticleNumber);
        #endregion Delete arranged resources
    }

    [TestMethod]
    public async Task Test_Find()
    {
        #region Arrange
        var tmpSupplier = await FortnoxClient.SupplierConnector.CreateAsync(new Supplier() { Name = "TmpSupplier" });
        var tmpArticle = await FortnoxClient.ArticleConnector.CreateAsync(new Article() { Description = "TmpArticle", PurchasePrice = 100 });
        #endregion Arrange

        var connector = FortnoxClient.SupplierInvoiceConnector;
        var newSupplierInvoice = new SupplierInvoice()
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
                new SupplierInvoiceRow(){ ArticleNumber = tmpArticle.ArticleNumber, Quantity = 10, Price = 100},
                new SupplierInvoiceRow(){ ArticleNumber = tmpArticle.ArticleNumber, Quantity = 20, Price = 100},
                new SupplierInvoiceRow(){ ArticleNumber = tmpArticle.ArticleNumber, Quantity = 20, Price = 100}
            }
        };

        //Add entries
        for (var i = 0; i < 5; i++)
        {
            await connector.CreateAsync(newSupplierInvoice);
        }

        //Apply base test filter
        var searchSettings = new SupplierInvoiceSearch();
        searchSettings.SupplierNumber = tmpSupplier.SupplierNumber;
        var fullCollection = await connector.FindAsync(searchSettings);

        Assert.AreEqual(5, fullCollection.TotalResources);
        Assert.AreEqual(5, fullCollection.Entities.Count);
        Assert.AreEqual(1, fullCollection.TotalPages);

        Assert.AreEqual(tmpSupplier.SupplierNumber, fullCollection.Entities.First().SupplierNumber);

        //Apply Limit
        searchSettings.Limit = 2;
        var limitedCollection = await connector.FindAsync(searchSettings);

        Assert.AreEqual(5, limitedCollection.TotalResources);
        Assert.AreEqual(2, limitedCollection.Entities.Count);
        Assert.AreEqual(3, limitedCollection.TotalPages);

        //Delete entries (DELETE not supported)
        foreach (var invoice in fullCollection.Entities)
            await connector.CancelAsync(invoice.GivenNumber);

        #region Delete arranged resources
        await FortnoxClient.SupplierConnector.DeleteAsync(tmpSupplier.SupplierNumber);
        await FortnoxClient.ArticleConnector.DeleteAsync(tmpArticle.ArticleNumber);
        #endregion Delete arranged resources
    }

    [TestMethod]
    public async Task Test_Book()
    {
        #region Arrange
        var tmpSupplier = await FortnoxClient.SupplierConnector.CreateAsync(new Supplier() { Name = "TmpSupplier" });
        var tmpArticle = await FortnoxClient.ArticleConnector.CreateAsync(new Article() { Description = "TmpArticle", PurchasePrice = 100 });
        #endregion Arrange

        var connector = FortnoxClient.SupplierInvoiceConnector;

        var newSupplierInvoice = new SupplierInvoice()
        {
            SupplierNumber = tmpSupplier.SupplierNumber,
            Comments = "InvoiceComments",
            InvoiceDate = new DateTime(2020, 1, 1),
            DueDate = new DateTime(2020, 2, 1),
            SalesType = SalesType.Stock,
            OCR = "1234567897",
            Total = 5000,
            SupplierInvoiceRows = new List<SupplierInvoiceRow>()
            {
                new SupplierInvoiceRow(){ ArticleNumber = tmpArticle.ArticleNumber, Quantity = 10, Price = 100},
                new SupplierInvoiceRow(){ ArticleNumber = tmpArticle.ArticleNumber, Quantity = 20, Price = 100},
                new SupplierInvoiceRow(){ ArticleNumber = tmpArticle.ArticleNumber, Quantity = 20, Price = 100}
            }
        };

        var createdSupplierInvoice = await connector.CreateAsync(newSupplierInvoice);

        //Act
        await connector.BookkeepAsync(createdSupplierInvoice.GivenNumber);
        var bookedInvoice = await connector.GetAsync(createdSupplierInvoice.GivenNumber);

        //Assert
        Assert.AreEqual(true, bookedInvoice.Booked);
        Assert.AreEqual(1, bookedInvoice.Vouchers.Count);
        Assert.AreEqual(ReferenceType.SupplierInvoice, bookedInvoice.Vouchers.First().ReferenceType);

        #region Delete arranged resources
        //Can not cancel booked invoice
        /*            connector.Cancel(createdSupplierInvoice.GivenNumber);
                    FortnoxClient.SupplierConnector.Delete(tmpSupplier.SupplierNumber);
                    FortnoxClient.ArticleConnector.Delete(tmpArticle.ArticleNumber);*/
        #endregion Delete arranged resources
    }
}