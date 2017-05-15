﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Database.Helpers
{
    public class SoftDeleteQueryVisitor : DefaultExpressionVisitor
    {
        public override DbExpression Visit(DbScanExpression expression)
        {
            var column = SoftDeleteAttribute.GetSoftDeleteColumnName(expression.Target.ElementType);
            if (column != null)
            {
                var table = (EntityType)expression.Target.ElementType;
                if (table.Properties.Any(p => p.Name == column))
                {
                    var binding = DbExpressionBuilder.Bind(expression);
                    return DbExpressionBuilder.Filter(
                        binding,
                        DbExpressionBuilder.NotEqual(
                            DbExpressionBuilder.Property(
                                DbExpressionBuilder.Variable(binding.VariableType, binding.VariableName),
                                column),
                            DbExpression.FromBoolean(true)));
                }
            }

            return base.Visit(expression);
        }
    }
}