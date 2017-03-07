using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Database
{
    public class Basic
    {
        public Basic()
        {
            Deleted = false;
            CreatedBy = 0;
            CreatedOn = DateTime.Now;
        }

        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Deleted { get; set; }
    }
}
