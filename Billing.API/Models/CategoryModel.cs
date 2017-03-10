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
            public double Price;

        }
        public CategoryModel()
        {
            Products = new List<CategoryProduct>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<CategoryProduct> Products { get; set; }
    }
}