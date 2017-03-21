using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models.Reports
{
    public class CategorySalesModel
    {
        public string Name { get; set; }
        public double Total { get; set; }
        public double Percent { get; set; }
    }

    public class SalesByCategoryModel
    {
        public SalesByCategoryModel(DateTime start, DateTime end)
        {
            StartDate = start;
            EndDate = end;
            Sales = new List<CategorySalesModel>();
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double GrandTotal { get; set; }
        public List<CategorySalesModel> Sales { get; set; }
    }
}