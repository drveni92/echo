using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;

namespace Billing.API.Helpers
{
    public static class ErrorGeneratorMessage
    {
        public static string Generate(DbEntityValidationException exception)
        {
            StringBuilder std = new StringBuilder();
            foreach (var errors in exception.EntityValidationErrors)
            {
                foreach (var validation in errors.ValidationErrors)
                {
                    std.Append(validation.ErrorMessage).AppendLine();
                }
            }
            return std.ToString();
        }
    }
}