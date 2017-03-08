using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models
{
    public class TownModel
    {

        public TownModel()
        {
            Customers = new List<string>();
            Suppliers = new List<string>();
            Shippers = new List<string>();
            Agents = new List<string>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
       
        public string Zip { get; set; }

        public  List<string> Customers { get; set; }
        public  List<string> Suppliers { get; set; }
        public  List<string> Shippers { get; set; }
        public  List<string> Agents { get; set; }
    }
}