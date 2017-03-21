using Billing.API.Models;
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

        public FactoryReports(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public DashboardModel Report()
        {
            int currentMonth = 4;
            DashboardModel result = new DashboardModel((int)Status.Delivered, (int)Region.Zenica);

            result.RegionsMonth = _unitOfWork.Invoices.Get()
                    .Where(x => x.Date.Month == currentMonth).ToList()
                    .GroupBy(x => x.Customer.Town.Region)
                    .Select(x => new MonthlySales()
                    { Label = x.Key.ToString(), Sales = x.Sum(y => (y.Total)) }).ToList();

            var listAnnual = _unitOfWork.Invoices.Get().ToList()
                    .GroupBy(x => new { x.Customer.Town.Region, x.Date.Month })
                    .Select(x => new {
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
                                .Select(x => new {
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

        public SalesByRegionModel Report(DateTime start, DateTime end, int agentId)
        {
            SalesByRegionModel result = new SalesByRegionModel();
            result.StartDate = start;
            result.EndDate = end;

            var Invoices = _unitOfWork.Invoices.Get().Where(x => (x.Date >= start && x.Date <= end)).ToList();

            result.GrandTotal = Invoices.Sum(x => x.Total);

            result.Sales = new List<RegionSalesModel>();

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

                region.Agents = new List<AgentSalesModel>();
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
                        RegionPercent = agent.Total / region.Total * 100,
                        TotalPercent = agent.Total / result.GrandTotal * 100
                    });
                }

                result.Sales.Add(region);
            }

            return result;

        }
    }
}