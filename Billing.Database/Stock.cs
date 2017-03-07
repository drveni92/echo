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
        public Stock()
        {
            Input = 0;
            Output = 0;
        }
        [Key]
        [ForeignKey("Product")]
        public int Id { get; set; }
        [Required]
        public int Input { get; set; }
        [Required]
        public int Output { get; set; }
        [NotMapped]
        public int Invertory { get { return Input - Output; } }
        [Required]
        public virtual Product Product { get; set; }

    }
}
