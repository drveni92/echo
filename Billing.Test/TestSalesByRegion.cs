using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing.API.Reports;
using Billing.Repository;
using Billing.API.Helpers.Identity;
using Billing.API.Models.Reports;

namespace Billing.Test
{
    [TestClass]
    public class TestSalesByRegion
    {
        private SalesByRegion report = new SalesByRegion(new UnitOfWork());

        private int regions = 2;
        private int agents = 7;
        private SalesByRegionModel result;

        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            TestHelper.InitDatabaseReports();
        }

        [TestInitialize]
        public void InitReport()
        {
            DateTime start = new DateTime(2016, 1, 1);
            DateTime end = new DateTime(2016, 12, 31);
            result = report.Report(start, end);
        }

        [TestMethod]
        public void CountNumberOfReturnedRegions()
        {
            Assert.AreEqual(result.Sales.Count, regions);
        }

        [TestMethod]
        public void CountNumberOfReturnedAgents()
        {
            int sum = 0;
            foreach (var item in result.Sales)
            {
                sum += item.Agents.Count;
            }
            Assert.AreEqual(sum, agents);
        }
    }
}
