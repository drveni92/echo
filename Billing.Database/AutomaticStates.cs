using System;
using System.Collections.Generic;
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
        public bool Completed { get; set; }
        public bool Checked { get; set; }
        public virtual Invoice Invoice { get; set; }

    }
}
