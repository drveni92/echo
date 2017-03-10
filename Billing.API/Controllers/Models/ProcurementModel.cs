using Billing.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models
{
    public class ProcurementModel
    {
        public struct ProcurementProduct
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public struct ProcurementSupplier
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }
        public string Document { get; set; }
        public ProcurementProduct Product { get; set; }
        public ProcurementSupplier Supplier { get; set; }
        public double Total { get { return Price * Quantity; } }
    }
}