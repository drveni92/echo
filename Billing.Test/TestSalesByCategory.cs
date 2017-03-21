using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing.API.Reports;
using Billing.Repository;

namespace Billing.Test
{
    [TestClass]
    public class TestSalesByCategory
    {
        private FactoryReports factory = new FactoryReports(new UnitOfWork());

        private int categories = 7;

        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            TestHelper.InitDatabaseReports();
        }

        [TestMethod]
        public void CountNumberOfReturnedRegions()
        {
            DateTime start = new DateTime(2016, 1, 1);
            DateTime end = new DateTime(2016, 12, 31);
            var result = factory.ReportRegion(start, end);
            Assert.AreEqual(result.Sales.Count, categories);
        }
    }
}
