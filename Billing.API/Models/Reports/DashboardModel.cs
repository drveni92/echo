using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models.Reports
{
    public class InputItem
    {
        public string Label { get; set; }
        public int Index { get; set; }
        public double Value { get; set; }
    }

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
        public AnnualSales(int Length = 12)
        {
            Sales = new double[Length];
        }
        public string Label { get; set; }
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

    public class InvoiceStatus
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }

    public class DashboardModel
    {
        public DashboardModel(int StatusCount, int RegionCount)
        {
            RegionsMonth = new List<MonthlySales>();
            RegionsYear = new List<AnnualSales>();
            CategoriesMonth = new List<MonthlySales>();
            CategoriesYear = new List<AnnualSales>();
            AgentsSales = new List<AnnualSales>(RegionCount);
            Top5Products = new List<ProductSales>();
            Invoices = new List<InvoiceStatus>();
            BurningItems = new List<BurningModel>();
            Customers = new List<CustomerStatus>();
        }

        public string Title { get; set; }
        public List<MonthlySales> RegionsMonth { get; set; }
        public List<AnnualSales> RegionsYear { get; set; }
        public List<MonthlySales> CategoriesMonth { get; set; }
        public List<AnnualSales> CategoriesYear { get; set; }
        public List<AnnualSales> AgentsSales { get; set; }

        public List<ProductSales> Top5Products { get; set; }
        public List<InvoiceStatus> Invoices { get; set; }
        public List<BurningModel> BurningItems { get; set; }
        public List<CustomerStatus> Customers { get; set; }

        public int NumberOfInvoices { get; set; }
        public double MonthTotal { get; set; }
        public int ShippedInvoices { get; set; }
        public int ActiveCustomers { get; set; }
    }
}