using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fortnox.SDK;
using Fortnox.SDK.Entities;
using Fortnox.SDK.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortnoxSDK.Tests.ConnectorTests;

[TestClass]
public class SupplierInvoiceExternalURLConnectionTests
{
    public FortnoxClient FortnoxClient = TestUtils.DefaultFortnoxClient;

    [TestMethod]
    public async Task Test_SupplierInvoiceExternalURLConnection_CRUD()
    {
        #region Arrange
        var tmpSupplier = await FortnoxClient.SupplierConnector.CreateAsync(new Supplier() { Name = "TmpSupplier" });
        var tmpArticle = await FortnoxClient.ArticleConnector.CreateAsync(new Article() { Description = "TmpArticle", PurchasePrice = 100 });
        var tmpSpplierInvoice = await FortnoxClient.SupplierInvoiceConnector.CreateAsync(new SupplierInvoice()
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
        });
        #endregion Arrange

        var connector = FortnoxClient.SupplierInvoiceExternalURLConnectionConnector;

        #region CREATE
        var newSupplierInvoiceExternalURLConnection = new SupplierInvoiceExternalURLConnection()
        {
            SupplierInvoiceNumber = (int?)tmpSpplierInvoice.GivenNumber,
            ExternalURLConnection = @"http://example.com/image.jpg"
        };

        var createdSupplierInvoiceExternalURLConnection = await connector.CreateAsync(newSupplierInvoiceExternalURLConnection);
        Assert.AreEqual("http://example.com/image.jpg", createdSupplierInvoiceExternalURLConnection.ExternalURLConnection);

        #endregion CREATE

        #region UPDATE

        createdSupplierInvoiceExternalURLConnection.ExternalURLConnection = "http://example.com/image.png";

        var updatedSupplierInvoiceExternalURLConnection = await connector.UpdateAsync(createdSupplierInvoiceExternalURLConnection);
        Assert.AreEqual("http://example.com/image.png", updatedSupplierInvoiceExternalURLConnection.ExternalURLConnection);

        #endregion UPDATE

        #region READ / GET

        var retrievedSupplierInvoiceExternalURLConnection = await connector.GetAsync(createdSupplierInvoiceExternalURLConnection.Id);
        Assert.AreEqual("http://example.com/image.png", retrievedSupplierInvoiceExternalURLConnection.ExternalURLConnection);

        #endregion READ / GET

        #region DELETE

        await connector.DeleteAsync(createdSupplierInvoiceExternalURLConnection.Id);

        await Assert.ThrowsExceptionAsync<FortnoxApiException>(
            async () => await connector.GetAsync(createdSupplierInvoiceExternalURLConnection.Id),
            "Entity still exists after Delete!");

        #endregion DELETE

        #region Delete arranged resources
        await FortnoxClient.SupplierInvoiceConnector.CancelAsync(tmpSpplierInvoice.GivenNumber);
        await FortnoxClient.ArticleConnector.DeleteAsync(tmpArticle.ArticleNumber);
        await FortnoxClient.SupplierConnector.DeleteAsync(tmpSupplier.SupplierNumber);
        #endregion Delete arranged resources
    }
}