using System.Linq;
using Fortnox.SDK;
using Fortnox.SDK.Entities;
using Fortnox.SDK.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortnoxSDK.Tests.ConnectorTests
{
    [TestClass]
    public class CurrencyTests
    {
        public FortnoxClient FortnoxClient = TestUtils.DefaultFortnoxClient;

        [TestMethod]
        public void Test_Currency_CRUD()
        {
            #region Arrange

            //Random currency code is not accepted by the server, therefore "SKK" is used.
            var currencyConnector = FortnoxClient.CurrencyConnector;
            try
            {
                //Delete currency if already exists
                currencyConnector.Delete("SKK");
            }
            catch
            {
                // Currency does not exists
            }

            #endregion Arrange

            var connector = FortnoxClient.CurrencyConnector;

            #region CREATE

            var newCurrency = new Currency()
            {
                Description = "TestCurrency",
                Code = "SKK",
                BuyRate = 1.11m,
                SellRate = 1.21m
            };

            var createdCurrency = connector.Create(newCurrency);
            Assert.AreEqual("TestCurrency", createdCurrency.Description);

            #endregion CREATE

            #region UPDATE

            createdCurrency.Description = "UpdatedCurrency";

            var updatedCurrency = connector.Update(createdCurrency);
            Assert.AreEqual("UpdatedCurrency", updatedCurrency.Description);

            #endregion UPDATE

            #region READ / GET

            var retrievedCurrency = connector.Get(createdCurrency.Code);
            Assert.AreEqual("UpdatedCurrency", retrievedCurrency.Description);

            #endregion READ / GET

            #region DELETE

            connector.Delete(createdCurrency.Code);

            Assert.ThrowsException<FortnoxApiException>(
                () => connector.Get(createdCurrency.Code),
                "Entity still exists after Delete!");

            #endregion DELETE

            #region Delete arranged resources

            //Add code to delete temporary resources

            #endregion Delete arranged resources
        }

        [TestMethod]
        public void Test_Currency_Find()
        {
            //Prerequisites: SEK, EUR and USD currencies are already present in the system

            var connector = FortnoxClient.CurrencyConnector;

            var currencies = connector.Find(null);

            Assert.AreEqual(3, currencies.Entities.Count); //SEK, EUR, USD
            Assert.AreEqual(true, currencies.Entities.Any(c => c.Code == "SEK"));
        }
    }
}
