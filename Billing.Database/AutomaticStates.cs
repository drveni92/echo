using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Database
{
    public class AutomaticStates : Basic
    {
        public AutomaticStates() { }
        public AutomaticStates(int _createdby)
        {
            Completed = false;
            Checked = false;
            CreatedBy = _createdby;
        }

        public int Id { get; set; }
        [Required]
        public bool Completed { get; set; }
        [Required]
        public bool Checked { get; set; }
        [Required]
        public virtual Invoice Invoice { get; set; }

    }
}
