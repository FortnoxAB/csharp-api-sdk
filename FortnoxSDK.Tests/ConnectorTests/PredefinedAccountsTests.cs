using System.Linq;
using Fortnox.SDK;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortnoxSDK.Tests.ConnectorTests
{
    [TestClass]
    public class PredefinedAccountsTests
    {
        public FortnoxClient FortnoxClient = TestUtils.DefaultFortnoxClient;

        [TestMethod]
        public void Test_PredefinedAccounts_CRUD()
        {
            var connector = FortnoxClient.PredefinedAccountsConnector;

            //Get
            var bygAccount = connector.Get("CONSTRUCTION_DEB");
            Assert.AreEqual(2647, bygAccount.Account);

            var patentAccount = connector.Get("PRODUCT_DEB");
            Assert.AreEqual(2645, patentAccount.Account);

            //Update
            patentAccount.Account = bygAccount.Account;
            connector.Update(patentAccount);
            Assert.AreEqual(2647, patentAccount.Account);

            //Revert
            patentAccount.Account = 2645;
            connector.Update(patentAccount);
            Assert.AreEqual(2645, patentAccount.Account);
        }

        [TestMethod]
        public void Test_Find()
        {
            var connector = FortnoxClient.PredefinedAccountsConnector;

            var fullCollection = connector.Find(null);

            Assert.AreEqual(42, fullCollection.Entities.Count);
            Assert.IsNotNull(fullCollection.Entities.First().Name);

            /* Limit not supported
            var searchSettings = new PredefinedAccountsSearch();
            searchSettings.Limit = 2;
            var limitedCollection = connector.Find(searchSettings);

            Assert.AreEqual(2, limitedCollection.Entities.Count);
            Assert.AreEqual(42, limitedCollection.TotalResources);
            */
        }
    }
}
