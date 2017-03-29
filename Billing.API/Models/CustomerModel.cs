using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models
{
    public class CustomerModel
    {
        public class CustomerTown
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public CustomerModel()
        {
            InvoicesNo = new List<string>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public CustomerTown Town { get; set; }
        public List<string> InvoicesNo { get; set; }
    }
}