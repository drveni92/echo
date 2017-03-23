using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing.API.Reports;
using Billing.Repository;
using Billing.API.Helper.Identity;

namespace Billing.Test
{
    [TestClass]
    public class TestReportAgentsRegionsCross
    {
        private FactoryReports factory = new FactoryReports(new UnitOfWork(), new BillingIdentity());

        private int agents = 8;
        private int regions = 8;
        private double total = 76884.21;

        [TestMethod]
        public void CountAgents()
        {
            DateTime start = new DateTime(2016, 1, 1);
            DateTime end = new DateTime(2016, 12, 31);
            var result = factory.ReportAgentsRegions(start, end);
            Assert.AreEqual(result.Agents.Count, agents);
        }

        [TestMethod]
        public void CountRegions()
        {
            DateTime start = new DateTime(2016, 1, 1);
            DateTime end = new DateTime(2016, 12, 31);
            var result = factory.ReportAgentsRegions(start, end);
            Assert.AreEqual(result.Regions.Count, regions);
        }

        [TestMethod]
        public void TestTotal()
        {
            DateTime start = new DateTime(2016, 1, 1);
            DateTime end = new DateTime(2016, 12, 31);
            var result = factory.ReportAgentsRegions(start, end);
            Assert.AreEqual(result.GrandTotal, total);
        }
    }
}
