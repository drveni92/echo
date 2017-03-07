using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.Database
{
    public class Invoice : Basic
    {
        public Invoice()
        {
            Items = new List<Item>();
            Date = DateTime.Now;
            ShippedOn = null;
        }
        public int Id { get; set; }

        [Required]
        public string InvoiceNo { get; set; }

        public DateTime Date { get; set; }

        public DateTime? ShippedOn { get; set; }

        [NotMapped]
        public double SubTotal { get
            {
                double temp = 0;
                foreach(Item item in Items)
                {
                    temp += item.SubTotal;
                }
                return temp;
            }
        }
        public double Vat { get; set; }

        [NotMapped]
        public double VatAmount { get { return Vat / 100f * SubTotal; } }

        public double Shipping { get; set; }
        public int Status { get; set; }

        [NotMapped]
        public double Total { get { return VatAmount + SubTotal; } }

        [Required]
        public virtual Agent Agent { get; set; }

        [Required]
        public virtual Shipper Shipper { get; set; }

        [Required]
        public virtual Customer Customer { get; set; }

        public virtual List<Item> Items { get; set; }
    }
}