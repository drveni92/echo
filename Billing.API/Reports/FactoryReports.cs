using Billing.API.Helpers.Identity;
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
        public MonthlySales Create(Region region, double sales)
        {
            return new MonthlySales()
            {
                Label = region.ToString(),
                Sales = sales
            };
        }

        public List<AnnualSales> Create(List<InputItem> list, int Length = 12)
        {
            List<AnnualSales> result = new List<AnnualSales>();
            AnnualSales current = new AnnualSales(Length);
            foreach (var item in list)
            {
                if (item.Label != current.Label)
                {
                    if (current.Label != null) result.Add(current);
                    current = new AnnualSales(Length);
                    current.Label = item.Label;
                }
                current.Sales[item.Index - 1] = item.Value;
            }
            if (current.Label != null) result.Add(current);
            return result;
        }

        public List<SalesAgentModel> Create(List<InputCross> list, List<Agent> agents, List<Region> regions)
        {
            List<SalesAgentModel> result = new List<SalesAgentModel>();
            foreach (var item in list)
            {
                SalesAgentModel agent = result.FirstOrDefault(x => x.Name == item.Row);
                if(agent == null)
                {
                    agent = new SalesAgentModel(regions);
                    agent.Name = item.Row;
                    result.Add(agent);
                }
                agent.Sales[item.Column] = item.Value;
                agent.Turnover += item.Value; 
            }
            foreach (var agent in agents)
            {
                if (!result.Exists(x => x.Name == agent.Name)) result.Add(new SalesAgentModel(regions)
                {
                    Name = agent.Name,
                    Turnover = 0
                });
            }
            return result;
        }

        public List<SalesRegionModel> CreateReverse(List<InputCross> list, List<Region> regions)
        {
            List<SalesRegionModel> result = new List<SalesRegionModel>();
            foreach (var region in regions)
            {
                result.Add(new SalesRegionModel()
                {
                    Region = region.ToString(),
                    Total = 0
                });
            }
            foreach (var item in list)
            {
                SalesRegionModel region = result.FirstOrDefault(x => x.Region == item.Column);
                if(region == null)
                {
                    region = new SalesRegionModel();
                    region.Region = item.Column;
                    result.Add(region);
                }
                region.Total += item.Value;
            }
            return result;
        }

        public CustomerSalesModel Create(int id, string name, double turnover, double grandTotal)
        {
            return new CustomerSalesModel()
            {
                Id = id,
                Name = name,
                Turnover = turnover,
                Percent = Math.Round(turnover / grandTotal * 100, 2)
            };
        }

        public RegionSalesModel Create(List<Invoice> Invoices, string Region, double Sales)
        {
            double GrandTotal = Invoices.Sum(x => x.SubTotal);
            RegionSalesModel region = new RegionSalesModel()
            {
                Name = Region,
                Total = Sales,
                Percent = Math.Round(100 * Sales / GrandTotal, 2)
            };
            region.Agents = Invoices.Where(x => x.Customer.Town.Region.ToString() == Region)
                           .GroupBy(x => new { id = x.Agent.Id, name = x.Agent.Name })
                           .Select(x => new AgentSalesModel()
                           {
                               Id = x.Key.id,
                               Name = x.Key.name,
                               Total = x.Sum(y => y.Total),
                               RegionPercent = 100 * x.Sum(y => y.Total) / Sales,
                               TotalPercent = 100 * x.Sum(y => y.Total) / GrandTotal
                           })
                           .ToList();
            return region;
        }

        public List<CustomerStatus> Customers(List<InputItem> list)
        {
            List<CustomerStatus> result = new List<CustomerStatus>();
            CustomerStatus current = new CustomerStatus();
            foreach (var item in list)
            {
                if (item.Label != current.Name)
                {
                    if (current.Name != null) result.Add(current);
                    current = new CustomerStatus();
                    current.Name = item.Label;
                }
                current.Debit += item.Value;
                if (item.Index > 3) current.Credit += item.Value;
            }
            if (current.Name != null) result.Add(current);
            return result.OrderByDescending(x => x.Debit).ToList();
        }

        public CustomerStatus Create(int Id, string Name, Status Status, double Amount)
        {
            return new CustomerStatus()
            {
                Id = Id,
                Name = Name,

            };
        }

        public ProductStockModel Create(int id, string name, Stock stock)
        {
            return new ProductStockModel()
            {
                Id = id,
                Name = name,
                Input = (stock != null) ? stock.Input : 0,
                Output = (stock != null) ? stock.Output : 0
            };
        }
    }
}