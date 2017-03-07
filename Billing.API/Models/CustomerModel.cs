using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models
{
    public class CustomerModel
    {
        public CustomerModel()
        {
            InvoicesNo = new List<string>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Town { get; set; }
        public List<string> InvoicesNo { get; set; }
    }
}