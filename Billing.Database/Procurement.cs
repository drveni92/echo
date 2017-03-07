using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.Database
{
    public class Procurement : Basic
    {
        public Procurement()
        {
            Quantity = 1;
        }
        public int Id { get; set; }
        public int Quantity { get; set; }

        [Required]
        public double Price { get; set; }

        public DateTime Date { get; set; }
        public string Document { get; set; }

        [Required]
        public virtual Product Product { get; set; }
        [Required]
        public virtual Supplier Supplier { get; set; }

        [NotMapped]
        public double Total { get { return Price * Quantity; } }

    }
}