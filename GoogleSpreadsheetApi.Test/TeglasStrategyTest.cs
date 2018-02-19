﻿using Exebite.Model;
using GoogleSpreadsheetApi.GoogleSSFactory;
using GoogleSpreadsheetApi.Strategies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace GoogleSpreadsheetApi.Test
{
    [TestClass]
    public class TeglasStrategyTest
    {
        static IRestaurantStrategy _strategy;
        static IGoogleSheetServiceFactory _ssFactory;
        static IGoogleSpreadsheetIdFactory _idFactory;
        static IUnityContainer _container;

        static List<Food> _menuData = new List<Food>();
        static List<Order> _orderData = new List<Order>();

        List<Food> _dailyMenu = new List<Food>();
        List<Order> _orders = new List<Order>();

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            _container = new UnityContainer();
            UnityConfig.RegisterTypes(_container);

            _ssFactory = _container.Resolve<IGoogleSheetServiceFactory>();
            _idFactory = _container.Resolve<IGoogleSpreadsheetIdFactory>();
            _strategy = new TeglasStrategy(_ssFactory, _idFactory);
            _menuData = _strategy.GetDailyMenu();
            _orderData = _strategy.GetHistoricalData();
        }


        [TestInitialize]
        public void Setup()
        {
            _dailyMenu.Clear();
            _dailyMenu.AddRange(_menuData);

            _orders.Clear();
            _orders.AddRange(_orderData);
        }

        [TestMethod]
        public void GetDailyMenu_NotEmpty()
        {
            Assert.AreNotEqual(_dailyMenu.Count, 0);
        }

        [TestMethod]
        public void GetDailyMenu_ItemValid()
        {
            Assert.IsNotNull(_dailyMenu[0].Name);
            Assert.IsNotNull(_dailyMenu[0].Price);
        }

        [TestMethod]
        public void GetHistoricalData_NotEmpty()
        {
            Assert.AreNotEqual(_orders.Count, 0);
        }
        [TestMethod]
        public void GetHistoricalData_ItemVaild()
        {
            Assert.IsNotNull(_orders[0].Customer);
            Assert.IsNotNull(_orders[0].Date);
            Assert.AreNotEqual(_orders[0].Meal.Foods.Count, 0);
        }
        
    }
}
