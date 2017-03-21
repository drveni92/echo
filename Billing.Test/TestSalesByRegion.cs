using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing.API.Reports;
using Billing.Repository;

namespace Billing.Test
{
    [TestClass]
    public class TestSalesByRegion
    {
        private FactoryReports factory = new FactoryReports(new UnitOfWork());

        private int regions = 2;
        private int agents = 7;

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
            Assert.AreEqual(result.Sales.Count, regions);
        }

        [TestMethod]
        public void CountNumberOfReturnedAgents()
        {
            DateTime start = new DateTime(2016, 1, 1);
            DateTime end = new DateTime(2016, 12, 31);
            var result = factory.ReportRegion(start, end);
            int sum = 0;
            foreach (var item in result.Sales)
            {
                sum += item.Agents.Count;
            }
            Assert.AreEqual(sum, agents);
        }
    }
}
