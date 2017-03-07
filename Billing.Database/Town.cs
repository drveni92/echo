using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.Database
{
    public class Town : Basic
    {
        public Town()
        {
            Customers = new List<Customer>();
            Suppliers = new List<Supplier>();
            Shippers = new List<Shipper>();
            Agents = new List<Agent>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public Region Region { get; set; }
        [MaxLength(20)]
        [Index(IsUnique = true)]
        public string Zip { get; set; }

        public virtual List<Customer> Customers { get; set; }
        public virtual List<Supplier> Suppliers { get; set; }
        public virtual List<Shipper> Shippers { get; set; }
        public virtual List<Agent> Agents { get; set; }
    }
}