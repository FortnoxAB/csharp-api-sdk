using System;
using System.Collections.Generic;
using Fortnox.SDK;
using Fortnox.SDK.Entities;
using Fortnox.SDK.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortnoxSDK.Tests.ConnectorTests
{
    [TestClass]
    public class SupplierInvoiceExternalURLConnectionTests
    {
        public FortnoxClient FortnoxClient = TestUtils.DefaultFortnoxClient;

        [TestMethod]
        public void Test_SupplierInvoiceExternalURLConnection_CRUD()
        {
            #region Arrange
            var tmpSupplier = FortnoxClient.SupplierConnector.Create(new Supplier() { Name = "TmpSupplier" });
            var tmpArticle = FortnoxClient.ArticleConnector.Create(new Article() { Description = "TmpArticle", PurchasePrice = 100 });
            var tmpSpplierInvoice = FortnoxClient.SupplierInvoiceConnector.Create(new SupplierInvoice()
            {
                SupplierNumber = tmpSupplier.SupplierNumber,
                Comments = "InvoiceComments",
                InvoiceDate = new DateTime(2020, 1, 1),
                DueDate = new DateTime(2020, 2, 1),
                SalesType = SalesType.Stock,
                OCR = "123456789",
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
                SupplierInvoiceNumber = (int?) tmpSpplierInvoice.GivenNumber,
                ExternalURLConnection = @"http://example.com/image.jpg"
            };

            var createdSupplierInvoiceExternalURLConnection = connector.Create(newSupplierInvoiceExternalURLConnection);
            Assert.AreEqual("http://example.com/image.jpg", createdSupplierInvoiceExternalURLConnection.ExternalURLConnection);

            #endregion CREATE

            #region UPDATE

            createdSupplierInvoiceExternalURLConnection.ExternalURLConnection = "http://example.com/image.png";

            var updatedSupplierInvoiceExternalURLConnection = connector.Update(createdSupplierInvoiceExternalURLConnection); 
            Assert.AreEqual("http://example.com/image.png", updatedSupplierInvoiceExternalURLConnection.ExternalURLConnection);

            #endregion UPDATE

            #region READ / GET

            var retrievedSupplierInvoiceExternalURLConnection = connector.Get(createdSupplierInvoiceExternalURLConnection.Id);
            Assert.AreEqual("http://example.com/image.png", retrievedSupplierInvoiceExternalURLConnection.ExternalURLConnection);

            #endregion READ / GET

            #region DELETE

            connector.Delete(createdSupplierInvoiceExternalURLConnection.Id);

            Assert.ThrowsException<FortnoxApiException>(
                () => connector.Get(createdSupplierInvoiceExternalURLConnection.Id),
                "Entity still exists after Delete!");

            #endregion DELETE

            #region Delete arranged resources
            FortnoxClient.SupplierInvoiceConnector.Cancel(tmpSpplierInvoice.GivenNumber);
            FortnoxClient.ArticleConnector.Delete(tmpArticle.ArticleNumber);
            FortnoxClient.SupplierConnector.Delete(tmpSupplier.SupplierNumber);
            #endregion Delete arranged resources
        }
    }
}
