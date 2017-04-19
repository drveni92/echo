using Billing.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Helpers
{
    public static class Helper
    {
        public static List<Region> Regions { get { return Enum.GetValues(typeof(Region)).Cast<Region>().ToList(); } }
        public static List<Status> Statuses { get { return Enum.GetValues(typeof(Status)).Cast<Status>().ToList(); } }

        public static string FirstLetterLow(string input)
        {
            return Char.ToLowerInvariant(input[0]) + input.Substring(1);
        }
    }
}