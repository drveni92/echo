using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing.API.Reports;
using Billing.Repository;
using Billing.API.Helpers.Identity;
using Billing.API.Models.Reports;

namespace Billing.Test
{
    [TestClass]
    public class TestReportCustomersCategoriesCross
    {
        private SalesByCustomersCategories report = new SalesByCustomersCategories(new UnitOfWork());

        private int customers = 10;
        private int categories = 12;
        private double total = 65713;
        SalesCustomersCategoriesModel result;

        [TestInitialize]
        public void InitReport()
        {
            DateTime start = new DateTime(2016, 1, 1);
            DateTime end = new DateTime(2016, 12, 31);
            result = report.Report(start, end);
        }

        [TestMethod]
        public void CountCustomers()
        {

            Assert.AreEqual(result.Customers.Count, customers);
        }

        [TestMethod]
        public void CountCategories()
        {
            Assert.AreEqual(result.Categories.Count, categories);
        }

        [TestMethod]
        public void TestTotal()
        {
            Assert.AreEqual(result.GrandTotal, total);
        }
    }
}
