using Billing.API.Models.Reports;
using Billing.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models.Reports
{
    public class SalesCustomersCategoriesModel
    {
        public SalesCustomersCategoriesModel(DateTime start, DateTime end)
        {
            StartDate = start;
            EndDate = end;
            Customers = new List<SalesCustomerModel>();
            Categories = new List<SalesCategoryModel>();
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Double GrandTotal { get; set; }
        public List<SalesCustomerModel> Customers { get; set; }
        public List<SalesCategoryModel> Categories { get; set; }
    }

    public class SalesCategoryModel
    {
        public string Category { get; set; }
        public double Total { get; set; }
    }

    public class SalesCustomerModel
    {
        public SalesCustomerModel(List<Category> categories)
        {
            Sales = new Dictionary<string, double>();
            foreach (var category in categories)
            {
                Sales[category.Name] = 0;
            }
        }
        public string Name { get; set; }
        public double Turnover { get; set; }
        public Dictionary<string, double> Sales { get; set; }
    }

}