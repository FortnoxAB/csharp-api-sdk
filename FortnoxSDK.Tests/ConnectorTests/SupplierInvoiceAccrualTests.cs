using System;
using System.Collections.Generic;
using Fortnox.SDK;
using Fortnox.SDK.Entities;
using Fortnox.SDK.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortnoxSDK.Tests.ConnectorTests
{
    [TestClass]
    public class SupplierInvoiceAccrualTests
    {
        public FortnoxClient FortnoxClient = TestUtils.DefaultFortnoxClient;

        [Ignore("Fails due to invoice not being balanced. Investigation needed")]
        [TestMethod]
        public void Test_SupplierInvoiceAccrual_CRUD()
        {
            #region Arrange
            var tmpSupplier = FortnoxClient.SupplierConnector.Create(new Supplier() { Name = "TmpSupplier" });
            var tmpArticle = FortnoxClient.ArticleConnector.Create(new Article() { Description = "TmpArticle" });
            var conn = FortnoxClient.SupplierInvoiceConnector;
            var tmpSupplierInvoice = conn.Create(new SupplierInvoice()
            {
                SupplierNumber = tmpSupplier.SupplierNumber,
                Comments = "InvoiceComments",
                InvoiceDate = new DateTime(2020, 1, 20), //"2019-01-20",
                DueDate = new DateTime(2020, 6, 20), //"2019-02-20",
                SalesType = SalesType.Stock,
                //OCR = "123456789",
                Total = 6000,
                SupplierInvoiceRows = new List<SupplierInvoiceRow>()
                {
                    new SupplierInvoiceRow(){ ArticleNumber = tmpArticle.ArticleNumber, Quantity = 6, Price = 1000, Account = 5820 }
                }
            });
            #endregion Arrange

            var connector = FortnoxClient.SupplierInvoiceAccrualConnector;

            #region CREATE
            var newSupplierInvoiceAccrual = new SupplierInvoiceAccrual()
            {
                Description = "TestSupplierInvoiceAccrual",
                SupplierInvoiceNumber = (int?) tmpSupplierInvoice.GivenNumber,
                Period = "MONTHLY",
                AccrualAccount = 1790,
                CostAccount = 5820,
                StartDate = new DateTime(2021, 1, 1),
                EndDate = new DateTime(2021,3, 1),
                VATIncluded = false,
                Total = 2000,
                SupplierInvoiceAccrualRows = new List<SupplierInvoiceAccrualRow>()
                {
                    new SupplierInvoiceAccrualRow(){ Account = 5820, Credit = 0, Debit = 2000 },
                    new SupplierInvoiceAccrualRow(){ Account = 1790, Credit = 2000, Debit = 0 }
                }
            };

            var createdSupplierInvoiceAccrual = connector.Create(newSupplierInvoiceAccrual);
            Assert.AreEqual("TestSupplierInvoiceAccrual", createdSupplierInvoiceAccrual.Description);

            #endregion CREATE

            #region UPDATE

            createdSupplierInvoiceAccrual.Description = "UpdatedTestSupplierInvoiceAccrual";

            var updatedSupplierInvoiceAccrual = connector.Update(createdSupplierInvoiceAccrual); 
            Assert.AreEqual("UpdatedTestSupplierInvoiceAccrual", updatedSupplierInvoiceAccrual.Description);

            #endregion UPDATE

            #region READ / GET

            var retrievedSupplierInvoiceAccrual = connector.Get(createdSupplierInvoiceAccrual.SupplierInvoiceNumber);
            Assert.AreEqual("UpdatedTestSupplierInvoiceAccrual", retrievedSupplierInvoiceAccrual.Description);

            #endregion READ / GET

            #region DELETE

            connector.Delete(createdSupplierInvoiceAccrual.SupplierInvoiceNumber);

            Assert.ThrowsException<FortnoxApiException>(
                () => connector.Get(createdSupplierInvoiceAccrual.SupplierInvoiceNumber),
                "Entity still exists after Delete!");
            #endregion DELETE

            #region Delete arranged resources
            FortnoxClient.SupplierConnector.Delete(tmpSupplier.SupplierNumber);
            FortnoxClient.ArticleConnector.Delete(tmpArticle.ArticleNumber);
            #endregion Delete arranged resources
        }
    }
}
