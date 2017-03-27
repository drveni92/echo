﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing.API.Reports;
using Billing.Repository;
using Billing.API.Models.Reports;

namespace Billing.Test
{
    [TestClass]
    public class TestStockLevel
    {
        private StockLevel report = new StockLevel(new UnitOfWork());


        private int categoryId = 2;
        private string categoryName = "DESKTOP";
        private int productsNo = 30;
        private StockLevelModel result;

        [TestInitialize]
        public void InitReport()
        {
            result = report.Report(categoryId);
        }

        [TestMethod]
        public void GetReportForCategory()
        {
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CheckCategoryId()
        {
            Assert.AreEqual(categoryId, result.CategoryId);
        }

        [TestMethod]
        public void CheckCategoryName()
        {
            Assert.AreEqual(categoryName.ToLower(), result.CategoryName.ToLower());
        }

        [TestMethod]
        public void CheckNumberOfProducts()
        {
            Assert.AreEqual(productsNo, result.Products.Count);
        }

        [TestMethod]
        public void CheckAllProductsInventory()
        {
            foreach (var item in result.Products)
            {
                Assert.AreEqual(item.Stock, item.Input - item.Output);
            }
        }
    }
}