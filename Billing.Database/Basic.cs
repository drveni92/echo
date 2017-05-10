using Billing.Database.Helpers;
using System;

namespace Billing.Database
{
    [SoftDelete]
    public abstract class Basic
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
