using Billing.API.Helpers;
using Billing.Database.Helpers;
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
        [StringValidation]
        public string Name { get; set; }

        [Required]
        [MinLength(4)]
        [StringValidation]
        public string Username { get; set; }

        public virtual List<Town> Towns { get; set; }
        public virtual List<Invoice> Invoices { get; set; }
    }
}
