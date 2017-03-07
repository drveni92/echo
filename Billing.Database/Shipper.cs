using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Billing.Database
{
    public class Shipper : Basic
    {
        public Shipper()
        {
            Invoices = new List<Invoice>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }

        public virtual Town Town { get; set; }
        public virtual List<Invoice> Invoices { get; set; }
    }
}