using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models
{
    public class ReturnObject<T>
    {
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public List<T> List { get; set; }
        public T Data { get; set; }
    }
}