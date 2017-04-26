using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models.Reports
{
    public class InvoiceReviewModel
    {
     
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? ShippedOn { get; set; }
        public double InvoiceTotal { get; set; }
        public int InvoiceStatus { get; set; }
    }

    public class InvoiceReviewCustomerModel
    {
        public InvoiceReviewCustomerModel()
        {
            Invoices = new List<InvoiceReviewModel>();
        }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double GrandTotal { get; set; }
        public List<InvoiceReviewModel> Invoices {get; set;}

    }

}