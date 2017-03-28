using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models
{
    public class AgentModel
    {
        public struct AgentTown
        {
            public int Id;
            public string Name;
        }
        public AgentModel()
        {
            Towns = new List<AgentTown>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public List<AgentTown> Towns { get; set; }
    }
}