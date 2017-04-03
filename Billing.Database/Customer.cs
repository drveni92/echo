using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Database
{
    public class Customer : Partner
    {
        public Customer()
        {
            Invoices = new List<Invoice>();
        }
        public virtual List<Invoice> Invoices { get; set; }
    }
}
