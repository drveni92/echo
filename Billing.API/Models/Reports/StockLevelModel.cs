using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models.Reports
{
    public class ProductStockModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Input { get; set; }
        public double Output { get; set; }
        public double Stock { get { return Input - Output; } }
    }
    public class StockLevelModel
    {
        public StockLevelModel(int _categoryId, string _name)
        {
            CategoryId = _categoryId;
            CategoryName = _name;
            Products = new List<ProductStockModel>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<ProductStockModel> Products { get; set; }
    }
}