using System;
using System.Collections.Generic;
using Fortnox.SDK;
using Fortnox.SDK.Entities;
using Fortnox.SDK.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortnoxSDK.Tests.ConnectorTests
{
    [TestClass]
    public class SupplierInvoicePaymentTests
    {
        public FortnoxClient FortnoxClient = TestUtils.DefaultFortnoxClient;

        [TestMethod]
        public void Test_SupplierInvoicePayment_CRUD()
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
            var bookedInvoice = FortnoxClient.SupplierInvoiceConnector.Bookkeep(tmpSpplierInvoice.GivenNumber);
            #endregion Arrange

            var connector = FortnoxClient.SupplierInvoicePaymentConnector;

            #region CREATE
            var newSupplierInvoicePayment = new SupplierInvoicePayment()
            {
                InvoiceNumber = (int?) tmpSpplierInvoice.GivenNumber,
                Amount = 1000,
                AmountCurrency = 1000,
                PaymentDate = new DateTime(2020, 1, 20)
            };

            var createdSupplierInvoicePayment = connector.Create(newSupplierInvoicePayment);
            Assert.AreEqual(1000, createdSupplierInvoicePayment.Amount);

            #endregion CREATE

            #region UPDATE

            createdSupplierInvoicePayment.Amount = 2000; 

            var updatedSupplierInvoicePayment = connector.Update(createdSupplierInvoicePayment); 
            Assert.AreEqual(2000, updatedSupplierInvoicePayment.Amount);

            #endregion UPDATE

            #region READ / GET

            var retrievedSupplierInvoicePayment = connector.Get(createdSupplierInvoicePayment.Number);
            Assert.AreEqual(2000, retrievedSupplierInvoicePayment.Amount);

            #endregion READ / GET

            #region DELETE

            connector.Delete(createdSupplierInvoicePayment.Number);

            Assert.ThrowsException<FortnoxApiException>(
                () => connector.Get(createdSupplierInvoicePayment.Number),
                "Entity still exists after Delete!");

            #endregion DELETE

            #region Delete arranged resources
            //Can't cancel invoice since it is booked
            //FortnoxClient.SupplierInvoiceConnector.Cancel(tmpSpplierInvoice.GivenNumber);
            //FortnoxClient.ArticleConnector.Delete(tmpArticle.ArticleNumber);
            //FortnoxClient.SupplierConnector.Delete(tmpSupplier.SupplierNumber);
            #endregion Delete arranged resources
        }
    }
}
