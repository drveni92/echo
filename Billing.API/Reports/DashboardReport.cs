using Billing.API.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Billing.Repository;
using Billing.Database;
using Billing.API.Helpers;
using Billing.API.Helpers.Identity;
using System.Security.Principal;

namespace Billing.API.Reports
{
    [TokenAuthorization("user")]
    public class DashboardReport : BaseReport
    {
        public DashboardReport(UnitOfWork unitOfWork) : base(unitOfWork) { }

        public DashboardModel Report()
        {
            int currentMonth = DateTime.Now.Month;
            DashboardModel result = new DashboardModel(Helper.Statuses.Count, Helper.Regions.Count);

            result.Title = "Dashboard for " + _identity.CurrentUser.Name;

            var tmp = _unitOfWork.Invoices.Get()
                     .Where(x => x.Date.Month == currentMonth &&
                     x.Agent.Id == _identity.CurrentUser.Id
                     ).ToList();

            if (_identity.HasRole("admin"))
            {
                tmp = _unitOfWork.Invoices.Get()
                     .Where(x => x.Date.Month == currentMonth).ToList();
            }

            result.RegionsMonth = tmp
                     .GroupBy(x => x.Customer.Town.Region)
                     .OrderBy(x => x.Key)
                     .Select(x => _factory.Create(x.Key, x.Sum(y => y.SubTotal))).ToList();

            List<InputItem> query;

            var tmp2 = _unitOfWork.Invoices.Get().Where(x => x.Agent.Id == _identity.CurrentUser.Id);
            if (_identity.HasRole("admin"))
            {
                tmp2 = _unitOfWork.Invoices.Get();
            }

            query = tmp2.OrderBy(x => x.Customer.Town.Region).ToList()
                                               .GroupBy(x => new { x.Customer.Town.Region, x.Date.Month })
                                               .Select(x => new InputItem { Label = x.Key.Region.ToString(), Index = x.Key.Month, Value = x.Sum(y => y.SubTotal) })
                                               .ToList();
            result.RegionsYear = _factory.Create(query);

            var tmp3 = _unitOfWork.Items.Get().Where(x => x.Invoice.Agent.Id == _identity.CurrentUser.Id);
            if (_identity.HasRole("admin"))
            {
                tmp3 = _unitOfWork.Items.Get();
            }

            query = tmp3.OrderBy(x => x.Product.Category.Id).ToList()
                .GroupBy(x => new { x.Product.Category.Name, x.Invoice.Date.Month })
                .Select(x => new InputItem { Label = x.Key.Name, Index = x.Key.Month, Value = x.Sum(y => y.SubTotal) })
                .ToList();
            result.CategoriesYear = _factory.Create(query);

            var tmp4 = _unitOfWork.Invoices.Get().Where(x => x.Agent.Id == _identity.CurrentUser.Id);
            if (_identity.HasRole("admin"))
            {
                tmp4 = _unitOfWork.Invoices.Get();
            }

            query = tmp4.OrderBy(x => x.Agent.Id).ToList()
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

            var custList = _unitOfWork.Invoices.Get()
                                      .OrderBy(x => x.Customer.Id).ToList()
                                      .GroupBy(x => new { x.Customer.Name, x.Status })
                                      .Select(x => new InputItem()
                                      {
                                          Label = x.Key.Name,
                                          Index = (int)x.Key.Status,
                                          Value = x.Sum(y => y.Total)
                                      })
                                      .ToList();
            result.Customers = _factory.Customers(custList).Take(5).ToList();

            result.BurningItems = _unitOfWork.Products.Get().ToList()
                                  .Select(x => new BurningModel() { Id = x.Id, Name = x.Name, Stock = (int)x.Stock.Inventory, Sold = (int)x.Stock.Output })
                                  .OrderByDescending(x => x.Sold)
                                  .Take(5)
                                  .ToList();
            return result;
        }
    }
}