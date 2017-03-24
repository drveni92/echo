using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing.Repository;
using Billing.API.Reports;
using Billing.API.Models.Reports;

namespace Billing.Test
{
 
    [TestClass]
    public class TestSalesByAgent
    {
        private SalesByRegion report = new SalesByRegion(new UnitOfWork());
        private int agentId= 6;
        private SalesByAgentModel result;

        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            TestHelper.InitDatabaseReports();
        }

        [TestInitialize]
        public void InitReport()
        {
            DateTime start = new DateTime(2016, 1, 1);
            DateTime end = new DateTime(2017, 12, 31);
            result = report.Report(start, end, agentId);
        }

        [TestMethod]
        public void CountNumberOfSalesInRegions()
        {
            double sum = 0;
            foreach (var item in result.Sales)
            {
                sum += item.RegionTotal;
            }

            Assert.AreEqual(result.AgentTotal, Math.Round(sum,2));

        }

        [TestMethod]
        public void RegionPercentAgentSales()
        {
            double sum = result.Sales[0].RegionTotal;
            double tot = result.AgentTotal;
            double per = Math.Round(sum / tot * 100, 2);

            Assert.AreEqual(result.Sales[0].RegionPercent, Math.Round(per, 2));

        }

        [TestMethod]
        public void TotalPercentAgentRegions()
        {
            double sum = 0;
            foreach (var item in result.Sales)
            {
                sum += item.TotalPercent;
            }

            Assert.AreEqual(result.PercentTotal, Math.Round(sum, 2));

        }

    }
}
