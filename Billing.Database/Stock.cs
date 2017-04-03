using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Database
{
    public class Stock : Basic
    {
        public int Id { get; set; }
        public double Input { get; set; }
        public double Output { get; set; }
        [NotMapped]
        public double Inventory { get { return (Input - Output); } }
        [Required]
        public virtual Product Product { get; set; }
    }
}
