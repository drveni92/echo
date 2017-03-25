﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models.Reports
{
    public class InvoiceItems
    {
      public int  ProductId { get; set; }

      public string ProductName { get; set; }

       public string ProductUnit { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public double Subtotal { get; set; }

    }

    public class InvoiceReportModel
    {
        public InvoiceReportModel()
        {
            Items = new List<InvoiceItems>();
        }
        public string InvoiceNo { get;  set; }

        public DateTime InvoiceDate { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string CustomerAddress { get; set; }

        //public string ZipCode { get; set; }

        public string Town { get; set; }

        public string Salesperson { get; set; }

        //public DateTime OrderDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public string ShippedVia { get; set; }

        public double InvoiceSubtotal { get; set; }

        public double VatAmount { get; set; }

        public double Shipping { get; set; }

        public double InvoiceTotal { get; set; }
        public List<InvoiceItems> Items { get; set; }

    }
}