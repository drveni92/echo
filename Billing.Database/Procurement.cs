using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.Database
{
    public class Procurement : Basic
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public string Document { get; set; }
        [Required]
        [Range(0, Int32.MaxValue)]
        public int Quantity { get; set; }
        [Required]
        [Range(0, Double.MaxValue)]
        public double Price { get; set; }
        [NotMapped]
        public double Total { get { return Math.Round(Quantity * Price, 2); } }

        [Required]
        public virtual Supplier Supplier { get; set; }
        [Required]
        public virtual Product Product { get; set; }
    }
}