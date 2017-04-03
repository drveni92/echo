using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Database
{
    public class Item : Basic
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        [NotMapped]
        public double SubTotal { get { return Math.Round(Quantity * Price, 2); } }

        [Required]
        public virtual Invoice Invoice { get; set; }
        [Required]
        public virtual Product Product { get; set; }
    }
}
