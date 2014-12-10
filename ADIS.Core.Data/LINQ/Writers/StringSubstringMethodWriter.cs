using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data.LINQ.Writers.Interfaces;

namespace ADIS.Core.Data.LINQ.Writers
{
    internal class StringSubstringMethodWriter : IMethodCallWriter
    {
        public bool CanHandle(MethodCallExpression expression)
        {
            return expression.Method.DeclaringType == typeof(string)
                       && expression.Method.Name == "Substring";
        }

        public string HandleQueryFragment(MethodCallExpression expression, Func<Expression, string> expressionWriter, Type sourceType)
        {
            var obj = expression.Object;

            if (expression.Arguments.Count == 1)
            {
                var argumentExpression = expression.Arguments[0];

                return string.Format(
                        "substring({0}, {1})", expressionWriter(obj), expressionWriter(argumentExpression));
            }

            var firstArgument = expression.Arguments[0];
            var secondArgument = expression.Arguments[1];

            return string.Format(
                    "substring({0}, {1}, {2})",
                    expressionWriter(obj),
                    expressionWriter(firstArgument),
                    expressionWriter(secondArgument));
        }

        public string HandleUriFragment(MethodCallExpression expression, Func<Expression, string> expressionWriter, Type sourceType)
        {
            var obj = expression.Object;

            if (expression.Arguments.Count == 1)
            {
                var argumentExpression = expression.Arguments[0];

                return string.Format(
                        "substring({0}, {1})", expressionWriter(obj), expressionWriter(argumentExpression));
            }

            var firstArgument = expression.Arguments[0];
            var secondArgument = expression.Arguments[1];

            return string.Format(
                    "substring({0}, {1}, {2})",
                    expressionWriter(obj),
                    expressionWriter(firstArgument),
                    expressionWriter(secondArgument));
        }
    }
}
