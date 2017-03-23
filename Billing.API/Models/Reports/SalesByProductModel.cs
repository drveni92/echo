using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models.Reports
{
    public class SalesByProductModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double CategoryTotal { get; set; }
        public double CategoryPercent { get; set; }
        public List<CategorySalesByProductModel> Sales { get; set; }
    }

    public class CategorySalesByProductModel
    {
        public string Name { get; set; }
        public double Total { get; set; }
        public double Percent { get; set; }
        public double TotalPercent { get; set; }
    }

}