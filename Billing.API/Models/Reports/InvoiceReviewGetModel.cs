using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models.Reports
{
    public class InvoiceReviewItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Subtotal { get; set; }
        public string Unit { get; set; }
    }

    public class InvoiceReviewGetModel
    {
        public InvoiceReviewGetModel()
        {
            Items = new List<InvoiceReviewItem>();
        }
        public string InvoiceNo { get; set; }
        public string CustomerName { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int InvoiceStatus { get; set; }
        public double Subtotal { get; set; }
        public double VatAmount { get; set; }
        public double Shipping { get; set; }
        public string Shipper { get; set; }
        public DateTime? ShippedOn { get; set; }
        public List<InvoiceReviewItem> Items { get; set; }
    }
}