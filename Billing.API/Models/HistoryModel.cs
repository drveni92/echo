using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models
{
    public class HistoryModel
    {
        public struct HistoryInvoice
        {
            public int Id;
            public string InvoiceNo;
        }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }
        public HistoryInvoice Invoice { get; set; }
    }
}