using System.Threading.Tasks;
using Fortnox.SDK;
using Fortnox.SDK.Entities;
using Fortnox.SDK.Exceptions;
using Fortnox.SDK.Search;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortnoxSDK.Tests.ConnectorTests;

[TestClass]
public class CustomerTests
{
    public FortnoxClient FortnoxClient = TestUtils.DefaultFortnoxClient;

    [TestMethod]
    public async Task Test_Customer_CRUD()
    {
        #region Arrange
        //Add code to create required resources
        #endregion Arrange

        var connector = FortnoxClient.CustomerConnector;

        #region CREATE
        var newCustomer = new Customer()
        {
            Name = "TestCustomer",
            Address1 = "TestStreet 1",
            Address2 = "TestStreet 2",
            ZipCode = "01010",
            City = "Testopolis",
            CountryCode = "SE", //CountryCode needs to be valid
            Email = "testCustomer@test.com",
            Type = CustomerType.Private,
            Active = false
        };

        var createdCustomer = await connector.CreateAsync(newCustomer);
        Assert.AreEqual("TestCustomer", createdCustomer.Name);

        #endregion CREATE

        #region UPDATE

        createdCustomer.Name = "UpdatedTestCustomer";

        var updatedCustomer = await connector.UpdateAsync(createdCustomer);
        Assert.AreEqual("UpdatedTestCustomer", updatedCustomer.Name);

        #endregion UPDATE

        #region READ / GET

        var retrievedCustomer = await connector.GetAsync(createdCustomer.CustomerNumber);
        Assert.AreEqual("UpdatedTestCustomer", retrievedCustomer.Name);

        #endregion READ / GET

        #region DELETE

        await connector.DeleteAsync(createdCustomer.CustomerNumber);

        await Assert.ThrowsExceptionAsync<FortnoxApiException>(
            async () => await connector.GetAsync(createdCustomer.CustomerNumber),
            "Entity still exists after Delete!");

        #endregion DELETE

        #region Delete arranged resources
        //Add code to delete temporary resources
        #endregion Delete arranged resources
    }

    [TestMethod]
    public async Task Test_Find()
    {
        #region Arrange
        //Add code to create required resources
        #endregion Arrange

        var testKeyMark = TestUtils.RandomString();
        var testPhone1 = "111111111";

        var connector = FortnoxClient.CustomerConnector;
        var newCustomer = new Customer()
        {
            Name = "TestCustomer",
            Address1 = "TestStreet 1",
            Address2 = "TestStreet 2",
            ZipCode = "01010",
            City = testKeyMark,
            CountryCode = "SE", //CountryCode needs to be valid
            Email = "testCustomer@test.com",
            Type = CustomerType.Private,
            Active = false,
            Comments = testKeyMark,
            Phone1 = testPhone1
        };

        //Add entries
        for (var i = 0; i < 5; i++)
        {
            await connector.CreateAsync(newCustomer);
        }

        //Apply base test filter
        var searchSettings = new CustomerSearch
        {
            City = testKeyMark,
            Phone = testPhone1
        };
        searchSettings.City = testKeyMark;
        var fullCollection = await connector.FindAsync(searchSettings);

        Assert.AreEqual(5, fullCollection.TotalResources);
        Assert.AreEqual(5, fullCollection.Entities.Count);
        Assert.AreEqual(1, fullCollection.TotalPages);

        //Apply Limit
        searchSettings.Limit = 2;
        var limitedCollection = await connector.FindAsync(searchSettings);

        Assert.AreEqual(5, limitedCollection.TotalResources);
        Assert.AreEqual(2, limitedCollection.Entities.Count);
        Assert.AreEqual(3, limitedCollection.TotalPages);

        //Delete entries
        foreach (var entry in fullCollection.Entities)
        {
            await connector.DeleteAsync(entry.CustomerNumber);
        }

        #region Delete arranged resources
        //Add code to delete temporary resources
        #endregion Delete arranged resources
    }
}