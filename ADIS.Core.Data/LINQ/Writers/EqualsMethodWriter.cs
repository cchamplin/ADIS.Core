using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data.LINQ.Writers.Interfaces;

namespace ADIS.Core.Data.LINQ.Writers
{
    internal class EqualsMethodWriter : IMethodCallWriter
    {
        public bool CanHandle(MethodCallExpression expression)
        {
            return expression.Method.Name == "Equals";
        }


        public string HandleQueryFragment(MethodCallExpression expression, Func<Expression, string> expressionWriter, Type sourceType)
        {
            return string.Format(
                   "{0} eq {1}",
                   expressionWriter(expression.Object),
                   expressionWriter(expression.Arguments[0]));
        }

        public string HandleUriFragment(MethodCallExpression expression, Func<Expression, string> expressionWriter, Type sourceType)
        {
            return string.Format(
                     "{0} eq {1}",
                     expressionWriter(expression.Object),
                     expressionWriter(expression.Arguments[0]));
        }
    }
}
