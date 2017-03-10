using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Models
{
    public class ProductModel
    {
        public struct ProductStock
        {
            public int Input;
            public int Output;
            public int Inventory;
        }

        public struct ProductCategory
        {
            public int Id;
            public string Name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public double Price { get; set; }
        public ProductStock Stock { get; set; }
        public ProductCategory Category { get; set; }
    }
}