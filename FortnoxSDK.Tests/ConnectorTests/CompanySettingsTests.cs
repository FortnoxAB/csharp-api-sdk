using Fortnox.SDK;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortnoxSDK.Tests.ConnectorTests
{
    [TestClass]
    public class CompanySettingsTests
    {
        public FortnoxClient FortnoxClient = TestUtils.DefaultFortnoxClient;

        [TestMethod]
        public void Test_CompanySettings_CRUD()
        {
            #region Arrange
            //Add code to create required resources
            #endregion Arrange

            var connector = FortnoxClient.CompanySettingsConnector;

            #region CREATE
            //Not Allowed
            #endregion CREATE

            #region UPDATE
            //Not Allowed
            #endregion UPDATE

            #region READ / GET

            var retrievedCompanySettings = connector.Get();
            Assert.IsNotNull(retrievedCompanySettings?.Name);

            #endregion READ / GET

            #region DELETE
            //Not Allowed
            #endregion DELETE

            #region Delete arranged resources
            //Add code to delete temporary resources
            #endregion Delete arranged resources
        }
    }
}
