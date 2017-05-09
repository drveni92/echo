using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.Database
{
    public class Town : Basic
    {
        public Town()
        {
            Suppliers = new List<Supplier>();
            Customers = new List<Customer>();
            Shippers = new List<Shipper>();
            Agents = new List<Agent>();
        }

        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        [Index(IsUnique = true)]
        public string Zip { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        [Required]
        public Region Region { get; set; }

        public virtual List<Supplier> Suppliers { get; set; }
        public virtual List<Customer> Customers { get; set; }
        public virtual List<Shipper> Shippers { get; set; }
        public virtual List<Agent> Agents { get; set; }
    }
}