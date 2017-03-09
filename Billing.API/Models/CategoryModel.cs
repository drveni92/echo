using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models
{
    public class CategoryModel
    {
        public struct CategoryProduct
        {
            public int Id;
            public string Name;
            public string Unit;
            public double price;

        }
        public CategoryModel()
        {
            Product = new List<CategoryProduct>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<CategoryProduct> Product { get; set; }
    }
}