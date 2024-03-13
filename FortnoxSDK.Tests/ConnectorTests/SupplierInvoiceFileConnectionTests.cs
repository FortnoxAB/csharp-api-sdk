using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fortnox.SDK;
using Fortnox.SDK.Entities;
using Fortnox.SDK.Exceptions;
using Fortnox.SDK.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortnoxSDK.Tests.ConnectorTests;

[TestClass]
public class SupplierInvoiceFileConnectionTests
{
    public FortnoxClient FortnoxClient = TestUtils.DefaultFortnoxClient;

    [TestMethod]
    public async Task Test_SupplierInvoiceFileConnection_CRUD()
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
        var tmpFile = await FortnoxClient.InboxConnector.UploadFileAsync("tmpImage.png", Resource.fortnox_image, StaticFolders.SupplierInvoices);
        #endregion Arrange

        var connector = FortnoxClient.SupplierInvoiceFileConnectionConnector;

        #region CREATE
        var newSupplierInvoiceFileConnection = new SupplierInvoiceFileConnection()
        {
            SupplierInvoiceNumber = tmpSpplierInvoice.GivenNumber.ToString(),
            FileId = tmpFile.Id
        };

        var createdSupplierInvoiceFileConnection = await connector.CreateAsync(newSupplierInvoiceFileConnection);
        Assert.AreEqual(tmpSupplier.Name, createdSupplierInvoiceFileConnection.SupplierName);

        #endregion CREATE

        #region UPDATE
        //Not supported
        #endregion UPDATE

        #region READ / GET

        var retrievedSupplierInvoiceFileConnection = await connector.GetAsync(createdSupplierInvoiceFileConnection.FileId);
        Assert.AreEqual(tmpSupplier.Name, retrievedSupplierInvoiceFileConnection.SupplierName);

        #endregion READ / GET

        #region DELETE

        await connector.DeleteAsync(createdSupplierInvoiceFileConnection.FileId);

        await Assert.ThrowsExceptionAsync<FortnoxApiException>(
            async () => await connector.GetAsync(createdSupplierInvoiceFileConnection.FileId),
            "Entity still exists after Delete!");

        #endregion DELETE

        #region Delete arranged resources
        await FortnoxClient.SupplierInvoiceConnector.CancelAsync(tmpSpplierInvoice.GivenNumber);
        await FortnoxClient.ArticleConnector.DeleteAsync(tmpArticle.ArticleNumber);
        await FortnoxClient.SupplierConnector.DeleteAsync(tmpSupplier.SupplierNumber);
        #endregion Delete arranged resources
    }
}