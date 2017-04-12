using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Helpers
{
    public static class Pagination
    {
        public static readonly int PageSize = 10;

        public static int GetTotalPages(int total)
        {
            return (int)Math.Ceiling((double)total / PageSize);
        }
    }
}