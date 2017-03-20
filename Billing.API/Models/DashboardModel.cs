using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models
{
    public class ProductSales
    {
        public string Product { get; set; }
        public int Quantity { get; set; }
        public double Revenue { get; set; }
    }
    public class MonthlySales
    {
        public string Label { get; set; }
        public double Sales { get; set; }
    }
    public class AnnualSales
    {
        public AnnualSales()
        {
            Sales = new double[12];
        }
        public string Label { get; set; }
        public double[] Sales { get; set; }
    }

    public class AgentsSales
    {
        public AgentsSales(int Length)
        {
            Sales = new double[Length];
        }
        public string Agent { get; set; }
        public double[] Sales { get; set; }
    }

    public class BurningModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public int Ordered { get; set; }
        public int Sold { get; set; }
    }

    public class CustomerStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Credit { get; set; }
        public double Debit { get; set; }
    }

    public class DashboardModel
    {
        public DashboardModel(int StatusesCount, int RegionsCount)
        {
            RegionsMonth = new List<MonthlySales>();
            RegionsYear = new List<AnnualSales>();
            CategoriesMonth = new List<MonthlySales>();
            CategoriesYear = new List<AnnualSales>();
            AgentsSales = new List<AgentsSales>(RegionsCount);
            Top5Products = new List<ProductSales>();
            Invoices = new int[StatusesCount];
            BurningItems = new List<BurningModel>();
            Customers = new List<CustomerStatus>();
        }

        public string Title { get; set; }
        public List<MonthlySales> RegionsMonth { get; set; }
        public List<AnnualSales> RegionsYear { get; set; }
        public List<MonthlySales> CategoriesMonth { get; set; }
        public List<AnnualSales> CategoriesYear { get; set; }
        public List<AgentsSales> AgentsSales { get; set; }

        public List<ProductSales> Top5Products { get; set; }
        public int[] Invoices { get; set; }
        public List<BurningModel> BurningItems { get; set; }
        public List<CustomerStatus> Customers { get; set; }
    }
}