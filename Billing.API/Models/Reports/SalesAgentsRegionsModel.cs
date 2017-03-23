using Billing.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models.Reports
{
    public class SalesRegionModel
    {
        public string Region { get; set; }
        public double Total { get; set; }
    }

    public class SalesAgentModel
    {
        public SalesAgentModel()
        {
            Sales = new Dictionary<Region, double>();
        }
        public string Name { get; set; }
        public double Turnover { get; set; }
        public Dictionary<Region, double> Sales { get; set; }
    }

    public class SalesAgentsRegionsModel
    {
        public SalesAgentsRegionsModel(DateTime start, DateTime end)
        {
            StartDate = start;
            EndDate = end;
            Agents = new List<SalesAgentModel>();
            Regions = new List<SalesRegionModel>();
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Double GrandTotal { get; set; }
        public List<SalesAgentModel> Agents { get; set; }
        public List<SalesRegionModel> Regions { get; set; }
    }
}