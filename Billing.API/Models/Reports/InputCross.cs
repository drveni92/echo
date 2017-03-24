using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models.Reports
{
    public class InputCross
    {
        public string Row { get; set; }
        public string Column { get; set; }
        public double Value { get; set; }
    }
}