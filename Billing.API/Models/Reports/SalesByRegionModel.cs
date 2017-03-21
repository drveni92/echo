using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models
{
    public class AgentSalesModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Total { get; set; }
        public double RegionPercent { get; set; }
        public double TotalPercent { get; set; }
    }

    public class RegionSalesModel
    {
        public RegionSalesModel()
        {
            Agents = new List<AgentSalesModel>();
        }

        public string Name { get; set; }
        public double Total { get; set; }
        public double Percent { get; set; }
        public List<AgentSalesModel> Agents { get; set; }
    }

    public class SalesByRegionModel
    {
        public SalesByRegionModel(DateTime start, DateTime end)
        {
            StartDate = start;
            EndDate = end;
            Sales = new List<RegionSalesModel>();
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double GrandTotal { get; set; }
        public List<RegionSalesModel> Sales { get; set; }
    }

}