using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models
{
    public class CategoryModel
    {
        public CategoryModel()
        {
            Products = new List<string>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public  List<string> Products { get; set; }
    }
}