using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models
{
    public class ShipperModel
    {
        public ShipperModel()
        {
            InvoicesNo = new List<string>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Town { get; set; }
        public List<string> InvoicesNo { get; internal set; }
    }
}