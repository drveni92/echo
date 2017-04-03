using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Database
{
    public class Supplier : Partner
    {
        public Supplier()
        {
            Procurements = new List<Procurement>();
        }
        public virtual List<Procurement> Procurements { get; set; }
    }
}
