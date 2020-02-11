using System;
using System.Collections.Generic;
using FortnoxAPILibrary.Connectors;
using FortnoxAPILibrary.Entities;
using FortnoxAPILibrary.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortnoxAPILibrary.GeneratedTests
{
    [TestClass]
    public class CurrencyTests
    {
        [TestInitialize]
        public void Init()
        {
            //Set global credentials for SDK
            //--- Open 'TestCredentials.resx' to edit the values ---\\
            ConnectionCredentials.AccessToken = TestCredentials.Access_Token;
            ConnectionCredentials.ClientSecret = TestCredentials.Client_Secret;
        }

        [TestMethod]
        public void Test_Currency_CRUD()
        {
            #region Arrange
            //Random currency code is not accepted by the server, therefore "SKK" is used.
            var currencyConnector = new CurrencyConnector();
            if (currencyConnector.Get("SKK") != null) //Delete currency if already exists
                currencyConnector.Delete("SKK");
            #endregion Arrange

            var connector = new CurrencyConnector();

            #region CREATE
            var newCurrency = new Currency()
            {
                Description = "TestCurrency",
                Code = "SKK",
                BuyRate = 1.11,
                SellRate = 1.21
            };

            var createdCurrency = connector.Create(newCurrency);
            MyAssert.HasNoError(connector);
            Assert.AreEqual("TestCurrency", createdCurrency.Description);

            #endregion CREATE

            #region UPDATE

            createdCurrency.Description = "UpdatedCurrency";

            var updatedCurrency = connector.Update(createdCurrency); 
            MyAssert.HasNoError(connector);
            Assert.AreEqual("UpdatedCurrency", updatedCurrency.Description);

            #endregion UPDATE

            #region READ / GET

            var retrievedCurrency = connector.Get(createdCurrency.Code);
            MyAssert.HasNoError(connector);
            Assert.AreEqual("UpdatedCurrency", retrievedCurrency.Description);

            #endregion READ / GET

            #region DELETE

            connector.Delete(createdCurrency.Code);
            MyAssert.HasNoError(connector);

            retrievedCurrency = connector.Get(createdCurrency.Code);
            Assert.AreEqual(null, retrievedCurrency, "Entity still exists after Delete!");

            #endregion DELETE

            #region Delete arranged resources
            //Add code to delete temporary resources
            #endregion Delete arranged resources
        }
    }
}
