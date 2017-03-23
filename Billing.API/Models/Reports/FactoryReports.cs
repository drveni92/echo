using Billing.API.Helper.Identity;
using Billing.API.Models;
using Billing.API.Models.Reports;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Reports
{
    public class FactoryReports
    {
        private UnitOfWork _unitOfWork;
        private BillingIdentity _identity;

        public FactoryReports(UnitOfWork unitOfWork, BillingIdentity identity)
        {
            _unitOfWork = unitOfWork;
            _identity = identity;
        }

        public DashboardModel Report()
        {
            int currentMonth = 4;
            DashboardModel result = new DashboardModel((int)Status.Delivered, (int)Region.Zenica);

            result.Title = "Dashboard for " + _identity.CurrentUser;

            result.RegionsMonth = _unitOfWork.Invoices.Get()
                    .Where(x => x.Date.Month == currentMonth).ToList()
                    .GroupBy(x => x.Customer.Town.Region)
                    .Select(x => new MonthlySales()
                    { Label = x.Key.ToString(), Sales = x.Sum(y => (y.Total)) }).ToList();

            var listAnnual = _unitOfWork.Invoices.Get().ToList()
                    .GroupBy(x => new { x.Customer.Town.Region, x.Date.Month })
                    .Select(x => new
                    {
                        label = x.Key.Region.ToString(),
                        month = x.Key.Month,
                        sales = x.Sum(y => (y.Total))
                    }).ToList();

            AnnualSales currentYear = new AnnualSales();
            foreach (var item in listAnnual)
            {
                if (item.label != currentYear.Label)
                {
                    if (currentYear.Label != null)
                        result.RegionsYear.Add(currentYear);
                    currentYear = new AnnualSales();
                    currentYear.Label = item.label;
                }
                currentYear.Sales[item.month - 1] = item.sales;
            }
            if (currentYear.Label != null) result.RegionsYear.Add(currentYear);

            var listCategories = _unitOfWork.Items.Get()
                                .OrderBy(x => x.Product.Category.Id).ToList()
                                .GroupBy(x => new { x.Product.Category.Name, x.Invoice.Date.Month })
                                .Select(x => new
                                {
                                    category = x.Key.Name,
                                    month = x.Key.Month,
                                    sales = x.Sum(y => y.SubTotal)
                                }).ToList();
            currentYear = new AnnualSales();
            foreach (var item in listCategories)
            {
                if (item.category != currentYear.Label)
                {
                    if (currentYear.Label != null) result.CategoriesYear.Add(currentYear);
                    currentYear = new AnnualSales();
                    currentYear.Label = item.category;
                }
                currentYear.Sales[item.month - 1] = item.sales;
            }
            if (currentYear.Label != null) result.CategoriesYear.Add(currentYear);

            var listAgents = _unitOfWork.Invoices.Get()
                            .OrderBy(x => new { agent = x.Agent.Id }).ToList()
                            .GroupBy(x => new { agent = x.Agent.Name, region = x.Customer.Town.Region })
                            .Select(x => new { label = x.Key.agent, region = (int)x.Key.region, sales = x.Sum(y => (y.Total)) })
                            .ToList();

            AgentsSales currentAgent = new AgentsSales((int)Region.Zenica);
            foreach (var item in listAgents)
            {
                if (item.label != currentAgent.Agent)
                {
                    if (currentAgent.Agent != null) result.AgentsSales.Add(currentAgent);
                    currentAgent = new AgentsSales((int)Region.Zenica);
                    currentAgent.Agent = item.label;
                }
                currentAgent.Sales[item.region - 1] = item.sales;
            }
            if (currentAgent.Agent != null) result.AgentsSales.Add(currentAgent);

            return result;
        }

        public SalesByRegionModel ReportRegion(DateTime start, DateTime end)
        {
            SalesByRegionModel result = new SalesByRegionModel(start, end);

            var Invoices = _unitOfWork.Invoices.Get().Where(x => (x.Date >= start && x.Date <= end)).ToList();

            result.GrandTotal = Invoices.Sum(x => x.Total);

            var query = Invoices.GroupBy(x => x.Customer.Town.Region.ToString())
                                .Select(x => new
                                {
                                    Name = x.Key,
                                    Total = x.Sum(y => y.Total)
                                }).ToList();

            foreach (var item in query)
            {
                RegionSalesModel region = new RegionSalesModel()
                {
                    Name = item.Name,
                    Total = item.Total,
                    Percent = item.Total / result.GrandTotal * 100,
                };

                var agents = Invoices.Where(x => x.Customer.Town.Region.ToString() == item.Name)
                                    .GroupBy(x => new
                                    {
                                        Id = x.Agent.Id,
                                        Name = x.Agent.Name
                                    })
                                    .Select(x => new
                                    {
                                        Id = x.Key.Id,
                                        Name = x.Key.Name,
                                        Total = x.Sum(y => y.Total)
                                    }).ToList();

                foreach (var agent in agents)
                {
                    region.Agents.Add(new AgentSalesModel()
                    {
                        Id = agent.Id,
                        Name = agent.Name,
                        Total = agent.Total,
                        RegionPercent = Math.Round(agent.Total / region.Total * 100,2),
                        TotalPercent = Math.Round(agent.Total / result.GrandTotal * 100,2)
                    });
                }

                result.Sales.Add(region);
            }

            return result;

        }

        public SalesByCustomerModel ReportCustomer(DateTime start, DateTime end)
        {
            SalesByCustomerModel result = new SalesByCustomerModel(start, end);

            var Invoices = _unitOfWork.Invoices.Get().Where(x => (x.Date >= start && x.Date <= end)).ToList();

            result.GrandTotal = Invoices.Sum(x => x.Total);

            var listOfCustomers = Invoices.GroupBy(x => new { Id = x.Customer.Id, Name = x.Customer.Name })
                                          .Select(x => new
                                          {
                                              Id = x.Key.Id,
                                              Name = x.Key.Name,
                                              Turnover = x.Sum(y => y.Items.Sum(z => z.Price * z.Quantity))
                                          }).ToList();

            foreach (var customer in listOfCustomers)
            {
                result.Customers.Add(new CustomerSalesModel()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Turnover = customer.Turnover,
                    Percent = Math.Round(customer.Turnover / result.GrandTotal * 100,2)
                });
            }


            return result;
        }

        public SalesByCategoryModel ReportCategory(DateTime start, DateTime end)
        {
            SalesByCategoryModel result = new SalesByCategoryModel(start, end);

            var Invoices = _unitOfWork.Invoices.Get().Where(x => (x.Date >= start && x.Date <= end)).ToList();
            var Items = Invoices.SelectMany(x => x.Items).ToList();
            result.GrandTotal = Invoices.Sum(x => x.SubTotal);

            var query = Items.GroupBy(x => x.Product.Category)
                              .Select(x => new { CategoryId = x.Key, CategoryTotal = x.Sum(y => y.SubTotal) })
                              .ToList();

            foreach (var item in query)
            {
                CategorySalesModel category = new CategorySalesModel()
                {
                    Name = item.CategoryId.Name,
                    Total = item.CategoryTotal,
                    Percent = Math.Round(item.CategoryTotal / result.GrandTotal * 100,2),
                };

                result.Sales.Add(category);
            }

            return result;

        }


        public InvoiceReviewCustomerModel ReportInvoicePost(int id, DateTime StartDate, DateTime EndDate)
        {
            InvoiceReviewCustomerModel result = new InvoiceReviewCustomerModel();
            result.StartDate = StartDate;
            result.EndDate = EndDate;
            result.CustomerId = id;
            var Invoices = _unitOfWork.Invoices.Get().Where(x => (x.Date >= StartDate && x.Date <= EndDate)).ToList();
            var Items = Invoices.SelectMany(x => x.Items).ToList();   

            var query2 = Invoices.Where(x => x.Customer.Id == id)
                .GroupBy(
                x => new {
                    Id = x.Id,
                    InvoiceNo = x.InvoiceNo,
                    Date = x.Date,
                    ShippedOn = x.ShippedOn,
                    Status = x.Status,
                    Vat = x.Vat,
                    Name = x.Customer.Name
                })
                              .Select(x => new
                              {
                                  Id = x.Key.Id,
                                  InvoiceNo = x.Key.InvoiceNo,
                                  Date = x.Key.Date,
                                  ShippedOn = x.Key.ShippedOn,
                                  Status = x.Key.Status,
                                  Vat = x.Key.Vat,                             
                                  Total = x.Sum(y => y.Total)
                              }).ToList();
            Customer customer = _unitOfWork.Customers.Get().FirstOrDefault(x => x.Id == id);
            result.CustomerName = customer.Name;
            double total = 0;
            foreach (var item in query2)
            {
                total += Math.Round((item.Total) / (1 + item.Vat / 100), 2);
                result.Invoices.Add(new InvoiceReviewModel()
                {
                    InvoiceId = item.Id,
                    InvoiceNo = item.InvoiceNo,
                    InvoiceTotal = Math.Round((item.Total)/(1+item.Vat/100),2),
                    InvoiceStatus = item.Status.ToString(),
                    InvoiceDate = item.Date,
                    ShippedOn = item.ShippedOn
                });
            }
            result.GrandTotal = Math.Round(total,2); 
          
            return result;
          
        }
              
        public SalesByAgentModel ReportAgentSales(DateTime start, DateTime end, int agentId)
        {
            SalesByAgentModel result = new SalesByAgentModel();
            var Invoices = _unitOfWork.Invoices.Get().Where(x => (x.Date >= start && x.Date <= end)).ToList();
            Agent a = _unitOfWork.Agents.Get(agentId);

            result.StartDate = start;
            result.EndDate = end;
            result.Sales = new List<RegionSalesByAgentModel>();
            result.AgentName = a.Name;
            double total = 0;
            double grandTotal = Invoices.Sum(x => x.Total);

            var query = Invoices.Where(x => x.Agent.Id == agentId).GroupBy(x => x.Customer.Town.Region.ToString())
                               .Select(x => new
                               {
                                   Name = x.Key,
                                   Total = x.Sum(y => y.Total)
                               }).ToList();

            foreach (var item in query)
            {
                total += item.Total;
            }

            result.AgentTotal = Math.Round(total,2);
            result.PercentTotal = Math.Round(100 * total / grandTotal, 2);

            foreach (var item in query)
            {
                RegionSalesByAgentModel region = new RegionSalesByAgentModel()
                {
                    RegionName = item.Name,
                    RegionTotal = Math.Round(item.Total,2),
                    RegionPercent =  Math.Round(100 * item.Total / total, 2),
                    TotalPercent = Math.Round(100 * item.Total / grandTotal, 2)
                };
                result.Sales.Add(region);

            }

            return result;
        }


        public InvoiceReviewGetModel ReportInvoiceGet(int id)
        {
            InvoiceReviewGetModel result = new InvoiceReviewGetModel();
            
            var Invoices = _unitOfWork.Invoices.Get().Where(x => (x.Id == id)).ToList();
            var Items = Invoices.SelectMany(x => x.Items).ToList();
            foreach (var invoice in Invoices)
            {
                result.InvoiceNo = invoice.InvoiceNo;
                result.InvoiceDate = invoice.Date;
                result.InvoiceStatus = invoice.Status.ToString();
                result.Subtotal = invoice.SubTotal;
                result.VatAmount = invoice.VatAmount;
                result.Shipping = invoice.Shipping;
                result.Shipper = invoice.Shipper.Name;
                result.ShippedOn = invoice.ShippedOn;

            }
                var query2 = Items//.Where(x => x. == id)
                .GroupBy(
                x => new {
                    ProductId = x.Product.Id,
                    ProductName = x.Product.Name,
                    Quantity = x.Quantity,
                    Price = x.Price,
                    Subtotal = x.SubTotal
                })
                              .Select(x => new
                              {               
                                  ProductId = x.Key.ProductId,
                                  ProductName = x.Key.ProductName,
                                  Quantity = x.Key.Quantity,
                                  Price = x.Key.Price,
                                  Subtotal = x.Key.Subtotal,
                                  Total = x.Sum(y => y.SubTotal)
                              }).ToList();
            var customer = Invoices.FirstOrDefault();
            result.CustomerName=customer.Customer.Name;

            foreach (var item in query2)
            {
                result.Items.Add(new InvoiceReviewItem()
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Subtotal = item.Subtotal
                   // Tota = item.Total
                });
            }


        public SalesAgentsRegionsModel ReportAgentsRegions(DateTime start, DateTime end)
        {
            SalesAgentsRegionsModel result = new SalesAgentsRegionsModel(start, end);

            var Invoices = _unitOfWork.Invoices.Get().Where(x => (x.Date >= start && x.Date <= end)).ToList();

            var Agents = _unitOfWork.Agents.Get().ToList();

            var Regions = Enum.GetValues(typeof(Region))
                              .Cast<Region>()
                              .ToList();

            foreach (var region in Regions)
            {
                result.Regions.Add(new SalesRegionModel()
                {
                    Region = region.ToString(),
                    Total = 0
                });
            }

            result.GrandTotal = Invoices.Sum(x => x.Total);

            foreach (var agent in Agents)
            {
                SalesAgentModel NewAgent = new SalesAgentModel();
                NewAgent.Name = agent.Name;
                NewAgent.Turnover = Invoices.Where(x => x.Agent.Id == agent.Id).Sum(x => x.Total);
                foreach (var region in Regions)
                {
                    NewAgent.Sales[region] = 0;
                }
                foreach (var town in Invoices.Where(x => x.Agent.Name == agent.Name).GroupBy(x => x.Customer.Town.Region).Select(x => new { Region = x.Key, Total = x.Sum(y => y.Total) }).ToList())
                {
                    NewAgent.Sales[town.Region] += Math.Round(town.Total, 2);
                    //add region list
                    var region = result.Regions.FirstOrDefault(x => x.Region == town.Region.ToString()).Total += Math.Round(town.Total, 2);

                }
                result.Agents.Add(NewAgent);
            }

            /*
            //Fill up Agents
            var Agents = Invoices.GroupBy(x => x.Agent.Name).ToList()
                                 .Select(x => new
                                 {
                                     Name = x.Key,
                                     Turnover = x.Sum(y => y.Total)
                                 }).ToList();

            foreach (var item in Agents)
            {
                SalesAgentModel agent = new SalesAgentModel();
                agent.Name = item.Name;
                agent.Turnover = Math.Round(item.Turnover, 2);
                foreach (var region in Regions)
                {
                    agent.Sales[region] = 0;
                }
                foreach (var town in Invoices.Where(x => x.Agent.Name == item.Name).GroupBy(x => x.Customer.Town.Region).ToList().Select(x => new { Region = x.Key, Total = x.Sum(y => y.Total)}))
                {
                    agent.Sales[town.Region] += Math.Round(town.Total, 2);
                    //add region list
                    var region = result.Regions.FirstOrDefault(x => x.Region == town.Region.ToString());
                    if (region != null)
                    {
                        if (!region.Agents.ContainsKey(agent.Name)) region.Agents.Add(agent.Name, 0);

                        region.Agents[agent.Name] += Math.Round(town.Total, 2);
                        region.Total += Math.Round(town.Total, 2);
                    }
                    else
                    {
                        SalesRegionModel reg = new SalesRegionModel();
                        reg.Region = town.Region.ToString();
                        reg.Total = Math.Round(town.Total, 2);
                        reg.Agents[agent.Name] = Math.Round(town.Total, 2);
                        result.Regions.Add(reg);
                    }
                }
                result.Agents.Add(agent);
            }
            */

            return result;
        }

        public SalesByProductModel ReportProduct(DateTime start, DateTime end, int CategoryId)
        {
            SalesByProductModel result = new SalesByProductModel();
            var Invoices = _unitOfWork.Invoices.Get().Where(x => (x.Date >= start && x.Date <= end)).ToList();
            var Items = Invoices.SelectMany(x => x.Items).ToList();
            Category a = _unitOfWork.Categories.Get(CategoryId);

            result.StartDate = start;
            result.EndDate = end;
            result.Sales = new List<CategorySalesByProductModel>();
            result.CategoryName = a.Name;
            double CategoryTotal = Items.Where(x => x.Product.Category.Id == CategoryId).Sum(x => x.SubTotal);
            double grandTotal = Invoices.Sum(x => x.Total);

            var query = Items.Where(x => x.Product.Category.Id == CategoryId).GroupBy(x => x.Product.Name)
                               .Select(x => new
                               {
                                   Name = x.Key,
                                   Total = x.Sum(y => y.SubTotal)
                               }).ToList();

            foreach (var item in query)
            {
                CategorySalesByProductModel product = new CategorySalesByProductModel()
                {
                    Name = item.Name,
                    Total = item.Total,
                    Percent = Math.Round(100 * item.Total / CategoryTotal, 2),
                    TotalPercent = Math.Round(100 * item.Total / grandTotal, 2)
                };
                result.Sales.Add(product);

            }
            return result;
        }

    }
}