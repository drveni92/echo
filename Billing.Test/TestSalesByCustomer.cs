using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing.Repository;
using Billing.API.Reports;

namespace Billing.Test
{
    [TestClass]
    public class TestSalesByCustomer
    {
        FactoryReports factory = new FactoryReports(new UnitOfWork());

        private int customers = 10;
        private double grandTotal = 76884.21;
        private double dec = 0.000001;

        [TestMethod]
        public void TestReturnedCustomers()
        {
            DateTime start = new DateTime(2016, 1, 1);
            DateTime end = new DateTime(2016, 12, 31);
            var result = factory.ReportCustomer(start, end);
            Assert.AreEqual(result.Customers.Count, customers);
        }

        [TestMethod]
        public void TestReturnedTotal()
        {
            DateTime start = new DateTime(2016, 1, 1);
            DateTime end = new DateTime(2016, 12, 31);
            var result = factory.ReportCustomer(start, end);
            Assert.IsTrue(Math.Abs(result.GrandTotal - grandTotal) < dec);
        }
    }
}
