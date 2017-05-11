using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models.Reports
{
    public class SalesByTowns
    {
        public SalesByTowns()
        {
            Agents = new List<AgentSalesModel>();
        }
        public string Region { get; set; }
        public string Town { get; set; }
        public List<AgentSalesModel> Agents { get; set; }
    }
}