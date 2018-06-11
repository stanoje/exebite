﻿using System;
using System.Linq;
using AutoMapper;
using Exebite.Business.Test.Mocks;
using Exebite.DataAccess;
using Exebite.DataAccess.Migrations;
using Exebite.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Exebite.Business.Test.Tests
{
    [TestClass]
    public class CustomerServiceTest
    {
        private static ICustomerService _customerService;
        private static IFoodOrderingContextFactory _factory;

        private static IMapper _mapper;


        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            var container = ServiceProviderWrapper.GetContainer();
            _factory = container.Resolve<IFoodOrderingContextFactory>();
            _mapper = container.Resolve<IMapper>();
            InMemoryDBSeed.Seed(_factory);
            _customerService = container.Resolve<ICustomerService>();
        }

        [TestMethod]
        public void GetAllCustomer()
        {
            var result = _customerService.GetAllCustomers();
            Assert.AreNotEqual(result.Count, 0);
        }

        [TestMethod]
        public void GetCustomerById()
        {
            var result = _customerService.GetCustomerById(1);
            Assert.AreEqual(result.Id, 1);
        }

        [TestMethod]
        public void GetCustomerById_NonExisting()
        {
            var result = _customerService.GetCustomerById(0);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetCustomerByIdentityId()
        {
            var appId = "TestAppUserId";
            var result = _customerService.GetCustomerByIdentityId(appId);
            Assert.AreEqual(result.AppUserId, appId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetCustomerByIdentityId_StringEmpity()
        {
            var appId = string.Empty;
            var result = _customerService.GetCustomerByIdentityId(appId);
        }

        [TestMethod]
        public void GetCustomerByIdentityId_NonExisting()
        {
            var appId = "NonExistingId";
            var result = _customerService.GetCustomerByIdentityId(appId);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetCustomerByName()
        {
            var name = "Test Customer";
            var result = _customerService.GetCustomerByName(name);
            Assert.AreEqual(result.Name, name);
        }

        [TestMethod]
        public void GetCustomerByName_NonExisting()
        {
            var name = "Non existing customer";
            var result = _customerService.GetCustomerByName(name);
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetCustomerByName_EmptyString()
        {
            _customerService.GetCustomerByName(string.Empty);
        }

        [TestMethod]
        public void CreateCustomer()
        {
            using (var context = _factory.Create())
            {
                Customer newCustomer = new Customer()
                {
                    AppUserId = "New App ID",
                    Balance = 0,
                    Name = "New Customer",
                    Location = _mapper.Map<Location>(context.Locations.First())
                };
                var result = _customerService.CreateCustomer(newCustomer);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateCustomer_Null()
        {
            _customerService.CreateCustomer(null);
        }

        [TestMethod]
        public void UpdateCustomerInfo()
        {
            using (var context = _factory.Create())
            {
                var newName = "New name";
                var newLocationId = 2;
                var customer = _customerService.GetAllCustomers().First();
                customer.Name = newName;
                customer.Location = _mapper.Map<Location>(context.Locations.Find(newLocationId));
                var result = _customerService.UpdateCustomer(customer);
                Assert.AreEqual(result.Name, newName);
                Assert.AreEqual(result.Location.Id, newLocationId);
            }
        }

        [TestMethod]
        public void DeleteCustomer()
        {
            var customerName = "Test Customer for delete";
            var customer = _customerService.GetCustomerByName(customerName);
            _customerService.DeleteCustomer(customer.Id);
            var result = _customerService.GetCustomerByName(customerName);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DeleteCustomer_NonExisting()
        {
            _customerService.DeleteCustomer(0);
        }
    }
}
