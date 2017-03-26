using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing.API.Reports;
using Billing.Repository;
using Billing.API.Models.Reports;

namespace Billing.Test
{
    [TestClass]
    public class TestInvoiceReport
    {
        private InvoiceReport report = new InvoiceReport(new UnitOfWork());
        private int InvoiceId = 5;
        private InvoiceReportModel result;

        [TestInitialize]
        public void InitReport()
        {
            result = report.Report(InvoiceId);
        }

        [TestMethod]
        public void CountSumInvoiceReport()
        {
            double sum = 0;
            foreach (var item in result.Items)
            {
                sum += item.Subtotal;
            }

            Assert.AreEqual(result.InvoiceSubtotal, Math.Round(sum, 2));

        }
        [TestMethod]
        public void InvoiceTotalInInvoiceReport()
        {       
            Assert.AreEqual(result.InvoiceTotal, result.InvoiceSubtotal + result.VatAmount);
        }
    }
}
