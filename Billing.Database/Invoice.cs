using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Billing.Database
{
    public class Invoice : Basic
    {
        public Invoice()
        {
            Items = new List<Item>();
            History = new List<Event>();
        }

        public int Id { get; set; }
        [Required]
        [MinLength(2)]
        public string InvoiceNo { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public DateTime? ShippedOn { get; set; }
        [Required]
        public Status Status { get; set; }
        [Required]
        public double Vat { get; set; }
        public double Shipping { get; set; }

        [NotMapped]
        public double SubTotal { get { return Math.Round(Items.Sum(x => x.Quantity * x.Price), 2); } }
        [NotMapped]
        public double VatAmount { get { return Math.Round(SubTotal * Vat / 100, 2); } }
        [NotMapped]
        public double Total { get { return Math.Round(SubTotal + VatAmount + Shipping, 2); } }

        [Required]
        public virtual Agent Agent { get; set; }
        [Required]
        public virtual Customer Customer { get; set; }
        public virtual Shipper Shipper { get; set; }

        public virtual List<Item> Items { get; set; }
        public virtual List<Event> History { get; set; }
    }
}