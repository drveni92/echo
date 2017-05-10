using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Database
{
    public class AuthToken : Basic
    {
        public int Id { get; set; }
        [Required]
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        [Required]
        public virtual ApiUser ApiUser { get; set; }
        public string Remember { get; set; }
        [Required]
        public virtual Agent Agent { get; set; }
    }
}
