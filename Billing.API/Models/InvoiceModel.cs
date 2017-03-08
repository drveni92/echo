﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models
{
    public class InvoiceModel
    {
        public InvoiceModel()
        {
            Items = new List<ItemModel>();
        }
        public struct InvoiceAgent
        {
            public int Id;
            public string Name;
        }
        public struct InvoiceShipper
        {
            public int Id;
            public string Name;
        }
        public struct InvoiceCustomer
        {
            public int Id;
            public string Name;
        }

        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime Date { get; set; }
        public DateTime? ShippedOn { get; set; }
        public double SubTotal { get; set; }
        public double Vat { get; set; }
        public double VatAmount { get; set; }
        public double Shipping { get; set; }
        public int Status { get; set; }
        public double Total { get; set; }
        public InvoiceAgent Agent { get; set; }
        public InvoiceShipper Shipper { get; set; }
        public InvoiceCustomer Customer { get; set; }
        public List<ItemModel> Items { get; set; }
    }
}