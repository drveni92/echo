using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Database
{
    public class Product : Basic
    {
        public Product()
        {
            Items = new List<Item>();
            Procurements = new List<Procurement>();
            Stock = new Stock()
            {
                Product = this
            };
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Unit { get; set; }
        [Required]
        public double Price { get; set; }

        [Required(ErrorMessage = "Please set Stock for Product")]
        public virtual Stock Stock { get; set; }

        [Required]
        public virtual Category Category { get; set; }

        public virtual List<Item> Items { get; set; }
        public virtual List<Procurement> Procurements { get; set; }
    }
}
