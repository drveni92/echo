using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models
{
    public class CurrentUserModel
    {
        public CurrentUserModel()
        {
            Roles = new List<string>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public List<string> Roles { get; set; }
    }
}