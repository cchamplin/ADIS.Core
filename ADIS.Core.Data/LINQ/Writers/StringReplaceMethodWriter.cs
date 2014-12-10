using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data.LINQ.Writers.Interfaces;

namespace ADIS.Core.Data.LINQ.Writers
{
    internal class StringReplaceMethodWriter : IMethodCallWriter
    {
        public bool CanHandle(MethodCallExpression expression)
        {
            return expression.Method.DeclaringType == typeof(string)
                       && expression.Method.Name == "Replace";
        }


        public string HandleQueryFragment(MethodCallExpression expression, Func<Expression, string> expressionWriter, Type sourceType)
        {
            var firstArgument = expression.Arguments[0];
            var secondArgument = expression.Arguments[1];
            var obj = expression.Object;



            return string.Format(
                    "replace({0}, {1}, {2})",
                    expressionWriter(obj),
                    expressionWriter(firstArgument),
                    expressionWriter(secondArgument));
        }

        public string HandleUriFragment(MethodCallExpression expression, Func<Expression, string> expressionWriter, Type sourceType)
        {
            var firstArgument = expression.Arguments[0];
            var secondArgument = expression.Arguments[1];
            var obj = expression.Object;



            return string.Format(
                    "replace({0}, {1}, {2})",
                    expressionWriter(obj),
                    expressionWriter(firstArgument),
                    expressionWriter(secondArgument));
        }
    }
}
