using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models.Reports
{
    public class CustomerSalesModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Turnover { get; set; }
        public double Percent { get; set; }
    }

    public class SalesByCustomerModel
    {
        public SalesByCustomerModel(DateTime start, DateTime end)
        {
            StartDate = start;
            EndDate = end;
            GrandTotal = 0;
            Customers = new List<CustomerSalesModel>();
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double GrandTotal { get; set; }
        public List<CustomerSalesModel> Customers { get; set; }
    }
}