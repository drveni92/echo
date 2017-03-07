﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Database
{
    [Table("Categories")]
    public class Category : Basic
    {
        public Category()
        {
            Products = new List<Product>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}
