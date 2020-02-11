using System;
using System.Collections.Generic;
using FortnoxAPILibrary.Connectors;
using FortnoxAPILibrary.Entities;
using FortnoxAPILibrary.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortnoxAPILibrary.GeneratedTests
{
    [TestClass]
    public class CustomerTests
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
        public void Test_Customer_CRUD()
        {
            #region Arrange
            //Add code to create required resources
            #endregion Arrange

            var connector = new CustomerConnector();

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
                Type = CustomerType.PRIVATE,
                Active = false
            };

            var createdCustomer = connector.Create(newCustomer);
            MyAssert.HasNoError(connector);
            Assert.AreEqual("TestCustomer", createdCustomer.Name);

            #endregion CREATE

            #region UPDATE

            createdCustomer.Name = "UpdatedTestCustomer";

            var updatedCustomer = connector.Update(createdCustomer); 
            MyAssert.HasNoError(connector);
            Assert.AreEqual("UpdatedTestCustomer", updatedCustomer.Name);

            #endregion UPDATE

            #region READ / GET

            var retrievedCustomer = connector.Get(createdCustomer.CustomerNumber);
            MyAssert.HasNoError(connector);
            Assert.AreEqual("UpdatedTestCustomer", retrievedCustomer.Name);

            #endregion READ / GET

            #region DELETE

            connector.Delete(createdCustomer.CustomerNumber);
            MyAssert.HasNoError(connector);

            retrievedCustomer = connector.Get(createdCustomer.CustomerNumber);
            Assert.AreEqual(null, retrievedCustomer, "Entity still exists after Delete!");

            #endregion DELETE

            #region Delete arranged resources
            //Add code to delete temporary resources
            #endregion Delete arranged resources
        }
    }
}
