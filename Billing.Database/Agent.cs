using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Database
{
    public class Agent : Basic
    {
        public Agent()
        {
            Towns = new List<Town>();
            Invoices = new List<Invoice>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Username { get; set; }

        public virtual List<Town> Towns { get; set; }
        public virtual List<Invoice> Invoices { get; set; }
    }
}
