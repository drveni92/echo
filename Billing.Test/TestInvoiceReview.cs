using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing.API.Reports;
using Billing.Repository;
using Billing.API.Models.Reports;

namespace Billing.Test
{
    [TestClass]
    public class TestInvoiceReview
    {
        private InvoicesReview report = new InvoicesReview(new UnitOfWork());
        private int customerId = 1;
        private InvoiceReviewCustomerModel result;

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
            result = report.Report(customerId, start, end);
        }

        [TestMethod]
        public void CountSumInvoicesInCustomer()
        {
            double sum = 0;
            foreach (var item in result.Invoices)
            {
                sum += item.InvoiceTotal;
            }

            Assert.AreEqual(result.GrandTotal, Math.Round(sum, 2));

        }
    }
}
