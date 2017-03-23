using Billing.API.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Billing.Repository;
using Billing.Database;
using Billing.API.Helpers;

namespace Billing.API.Reports
{
    public class DashboardReport : BaseReport
    {
        public DashboardReport(UnitOfWork unitOfWork) : base(unitOfWork) {}

        public DashboardModel Report()
        {
            int currentMonth = 4;
            DashboardModel result = new DashboardModel(Helper.Statuses.Count, Helper.Regions.Count);

            result.Title = "Dashboard for " + _identity.CurrentUser;

            result.RegionsMonth = _unitOfWork.Invoices.Get()
                    .Where(x => x.Date.Month == currentMonth).ToList()
                    .OrderBy(x => x.Customer.Town.Region)
                    .GroupBy(x => x.Customer.Town.Region)
                    .Select(x => _factory.Create(x.Key, x.Sum(y => y.SubTotal))).ToList();

            List<InputItem> query;

            query = _unitOfWork.Invoices.Get().OrderBy(x => x.Customer.Town.Region).ToList()
                    .GroupBy(x => new { x.Customer.Town.Region, x.Date.Month })
                    .Select(x => new InputItem { Label = x.Key.Region.ToString(), Index = x.Key.Month, Value = x.Sum(y => y.SubTotal) })
                    .ToList();
            result.RegionsYear = _factory.Create(query);


            query = _unitOfWork.Items.Get().OrderBy(x => x.Product.Category.Id).ToList()
                    .GroupBy(x => new { x.Product.Category.Name, x.Invoice.Date.Month })
                    .Select(x => new InputItem { Label = x.Key.Name, Index = x.Key.Month, Value = x.Sum(y => y.SubTotal) })
                    .ToList();
            result.CategoriesYear = _factory.Create(query);

            query = _unitOfWork.Invoices.Get().OrderBy(x => x.Agent.Id).ToList()
                    .GroupBy(x => new { agent = x.Agent.Name, region = x.Customer.Town.Region })
                    .Select(x => new InputItem { Label = x.Key.agent, Index = (int)x.Key.region, Value = x.Sum(y => (y.Total)) })
                    .ToList();
            result.AgentsSales = _factory.Create(query, Helper.Regions.Count);

            result.Top5Products = _unitOfWork.Items.Get().OrderBy(x => x.Product.Id).ToList()
                                  .GroupBy(x => x.Product.Name)
                                  .Select(x => new ProductSales() { Product = x.Key, Quantity = x.Sum(y => y.Quantity), Revenue = x.Sum(y => y.SubTotal) })
                                  .OrderByDescending(x => x.Revenue).Take(5)
                                  .ToList();

            result.Invoices = _unitOfWork.Invoices.Get().OrderBy(x => x.Status).ToList()
                              .GroupBy(x => x.Status.ToString())
                              .Select(x => new InvoiceStatus() { Status = x.Key, Count = x.Count() })
                              .ToList();

            result.Customers = _unitOfWork.Invoices.Get().ToList()
                               .GroupBy(x => new { x.Customer.Id, x.Customer.Name })
                               .Select(x => new CustomerStatus() { Id = x.Key.Id, Name = x.Key.Name, Credit = x.Sum(y => y.Total), Debit = x.Sum(y => y.Total) })
                               .OrderByDescending(x => x.Credit)
                               .ToList();

            result.BurningItems = _unitOfWork.Products.Get().ToList()
                                  .Select(x => new BurningModel() { Id = x.Id, Name = x.Name, Stock = (int)x.Stock.Invertory, Sold = (int)x.Stock.Output })
                                  //.OrderByDescending(x => x.Sold).Take(5)
                                  .ToList();
            return result;
        }
    }
}