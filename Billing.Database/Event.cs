using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Database
{
    public class Event : Basic
    {
        public int Id { get; set; }
        [Required]
        public Status Status { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public virtual Invoice Invoice { get; set; }
    }
}
