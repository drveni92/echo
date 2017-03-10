using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models
{
    public class TownModel
    {
        public struct TownCustomer
        {
            public int Id;
            public string Name;
            public string Adress;

        }
        public TownModel()
        {
            Customers = new List<TownCustomer>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }    
        public string Zip { get; set; } 
        public List<TownCustomer> Customers { get; set; }
    }
}