﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing.API.Reports;
using Billing.Repository;
using Billing.API.Helpers.Identity;

namespace Billing.Test
{
    [TestClass]
    public class TestSalesByCategory
    {
        private SalesByCategory report = new SalesByCategory(new UnitOfWork());

        private int categories = 7;

        [TestMethod]
        public void CountNumberOfReturnedCategories()
        {
            DateTime start = new DateTime(2016, 1, 1);
            DateTime end = new DateTime(2016, 12, 31);
            var result = report.Report(start, end);
            Assert.AreEqual(result.Sales.Count, categories);
        }
    }
}
