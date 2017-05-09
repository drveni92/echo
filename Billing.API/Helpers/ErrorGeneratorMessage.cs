using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace Billing.API.Helpers
{
    public static class ErrorGeneratorMessage
    {
        public static string Generate(DbEntityValidationException exception)
        {
            string result = "";
            foreach (var errors in exception.EntityValidationErrors)
            {
                foreach (var validation in errors.ValidationErrors)
                {
                    result += validation.ErrorMessage;
                    result += Environment.NewLine;
                }
            }
            return result;
        }
    }
}