using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models
{
    public class StockModel
    {
        public struct StockProduct
        {
            public int Id;
            public string Name;
        }
        public int Id { get; set; }
        public double Input { get; set; }
        public double Output { get; set; }
        public double Inventory { get { return Input - Output; } }
        public StockProduct Product { get; set; }
    }
}