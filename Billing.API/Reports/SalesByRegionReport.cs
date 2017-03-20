using Billing.API.Models;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Reports
{
    public static class SalesByRegionReport
    {
        public static SalesByRegionModel Report(UnitOfWork unitOfWork, DateTime start, DateTime end, int agentId)
        {
            SalesByRegionModel result = new SalesByRegionModel();
            result.StartDate = start;
            result.EndDate = end;

            var Invoices = unitOfWork.Invoices.Get().Where(x => (x.Date >= start && x.Date <= end)).ToList();

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