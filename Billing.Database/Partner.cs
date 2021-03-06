﻿using Billing.API.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Billing.Database
{
    public class Partner : Basic
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [StringValidation]
        public string Name { get; set; }
        public string Address { get; set; }
        [Required]
        public virtual Town Town { get; set; }
    }
}