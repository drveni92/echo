using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Database
{
    public class AutomaticStates : Basic
    {
        public AutomaticStates()
        {
            Completed = false;
        }

        public int Id { get; set; }
        public bool Completed { get; set; }
        public virtual Invoice Invoice { get; set; }

    }
}
