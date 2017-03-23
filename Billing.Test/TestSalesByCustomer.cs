using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing.Repository;
using Billing.API.Reports;
using Billing.API.Helpers.Identity;
using Billing.API.Models.Reports;

namespace Billing.Test
{
    [TestClass]
    public class TestSalesByCustomer
    {
        private SalesByCustomer report = new SalesByCustomer(new UnitOfWork());

        private int customers = 10;
        private double grandTotal = 76884.21;
        private double dec = 0.000001;
        SalesByCustomerModel result;

        [TestInitialize]
        public void InitReport()
        {
            DateTime start = new DateTime(2016, 1, 1);
            DateTime end = new DateTime(2016, 12, 31);
            result = report.Report(start, end);
        }

        [TestMethod]
        public void TestReturnedCustomers()
        {
            Assert.AreEqual(result.Customers.Count, customers);
        }

        [TestMethod]
        public void TestReturnedTotal()
        {
            Assert.IsTrue(Math.Abs(result.GrandTotal - grandTotal) < dec);
        }
    }
}
