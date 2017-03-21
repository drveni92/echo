using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models.Reports
{
    public class RequestModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Id { get; set; }
    }
}