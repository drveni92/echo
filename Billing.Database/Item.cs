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
        public Item()
        {
            Quantity = 1;
        }
        public int Id { get; set; }
        public int Quantity { get; set; }

        [Required]
        public double Price { get; set; }

        [NotMapped]
        public double SubTotal { get { return Quantity * Price; } }

        public virtual Product Product { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}
