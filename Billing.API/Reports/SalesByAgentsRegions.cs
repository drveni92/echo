using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Billing.Repository;
using Billing.API.Models.Reports;
using Billing.Database;

namespace Billing.API.Reports
{
    public class SalesByAgentsRegions : BaseReport
    {
        public SalesByAgentsRegions(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public SalesAgentsRegionsModel Report(DateTime start, DateTime end)
        {
            SalesAgentsRegionsModel result = new SalesAgentsRegionsModel(start, end);

            var Invoices = _unitOfWork.Invoices.Get().Where(x => (x.Date >= start && x.Date <= end)).ToList();

            var Agents = _unitOfWork.Agents.Get().ToList();

            var Regions = Helpers.Helper.Regions;

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

            return result;
        }
    }
}