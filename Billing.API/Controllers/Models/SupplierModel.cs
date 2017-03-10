using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models
{
    public class SupplierModel
    {
        public struct SupplierTown
        {
            public int Id;
            public string Name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public SupplierTown Town { get; set; }
    }
}