using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data.LINQ.Writers.Interfaces;

namespace ADIS.Core.Data.LINQ.Writers
{
    internal class StringToLowerMethodWriter : IMethodCallWriter
    {
        public bool CanHandle(MethodCallExpression expression)
        {
            return expression.Method.DeclaringType == typeof(string)
                       && (expression.Method.Name == "ToLower" || expression.Method.Name == "ToLowerInvariant");
        }

        public string HandleQueryFragment(MethodCallExpression expression, Func<Expression, string> expressionWriter, Type sourceType)
        {
            var obj = expression.Object;
            return string.Format("tolower({0})", expressionWriter(obj));
        }

        public string HandleUriFragment(MethodCallExpression expression, Func<Expression, string> expressionWriter, Type sourceType)
        {
            var obj = expression.Object;
            return string.Format("tolower({0})", expressionWriter(obj));
        }
    }
}
