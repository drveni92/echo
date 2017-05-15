using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Billing.API.Helpers
{
    /* Allows letters, numbers, dots, commas and space */
    public class StringValidationAttribute : ValidationAttribute
    {
        protected string pattern;
        private Regex regex;

        public StringValidationAttribute()
        {
            pattern = @"^[a-zA-Z0-9,. ČĆŽŠĐčćžšđ]*$";
            regex = new Regex(pattern, RegexOptions.IgnoreCase);
        }

        public override bool IsValid(object value)
        {
            string str = value as string;
            if(!string.IsNullOrEmpty(str))
            {
                if (regex.IsMatch(str)) return true;
                else
                {
                    ErrorMessage = "Invalid format";
                    return false;
                }
            }
            return true;
        }
    }
}