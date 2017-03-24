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

            List<Agent> Agents = _unitOfWork.Agents.Get().ToList();

            List<InputCross> AgentsByRegions = _unitOfWork.Invoices.Get()
                                               .Where(x => (x.Date >= start && x.Date <= end)).ToList()
                                               .GroupBy(x => new
                                               {
                                                   AgentName = x.Agent.Name,
                                                   RegionName = x.Customer.Town.Region.ToString()
                                               })
                                               .Select(x => new InputCross { Row = x.Key.AgentName, Column = x.Key.RegionName, Value = x.Sum(y => y.Total) })
                                               .OrderByDescending(x => x.Value)
                                               .ToList();

            result.Agents = _factory.Create(AgentsByRegions, Agents, Helpers.Helper.Regions);
            result.Regions = _factory.CreateReverse(AgentsByRegions, Helpers.Helper.Regions);
            result.GrandTotal = result.Agents.Sum(x => x.Turnover);

            return result;
        }
    }
}