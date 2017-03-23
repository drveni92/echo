using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing.API.Reports;
using Billing.Repository;
using Billing.API.Helpers.Identity;
using Billing.API.Models.Reports;

namespace Billing.Test
{
    [TestClass]
    public class TestReportAgentsRegionsCross
    {
        private SalesByAgentsRegions report = new SalesByAgentsRegions(new UnitOfWork());

        private int agents = 8;
        private int regions = 8;
        private double total = 76884.21;
        SalesAgentsRegionsModel result;

        [TestInitialize]
        public void InitReport()
        {
            DateTime start = new DateTime(2016, 1, 1);
            DateTime end = new DateTime(2016, 12, 31);
            result = report.Report(start, end);
        }

        [TestMethod]
        public void CountAgents()
        {
            
            Assert.AreEqual(result.Agents.Count, agents);
        }

        [TestMethod]
        public void CountRegions()
        {
            Assert.AreEqual(result.Regions.Count, regions);
        }

        [TestMethod]
        public void TestTotal()
        {
            Assert.AreEqual(result.GrandTotal, total);
        }
    }
}
