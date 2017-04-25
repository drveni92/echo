using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models
{
    public class SearchInvoice
    {
        public string Agent { get; set; }
        public string Customer { get; set; }
        public string Status { get; set; }
    }
}