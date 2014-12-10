using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data.LINQ.Writers.Interfaces;

namespace ADIS.Core.Data.LINQ.Writers
{
    internal class AnyAllMethodWriter : IMethodCallWriter
    {
        public bool CanHandle(MethodCallExpression expression)
        {
            return expression.Method.Name == "Any" || expression.Method.Name == "All";
        }


        public string HandleQueryFragment(MethodCallExpression expression, Func<Expression, string> expressionWriter, Type sourceType)
        {
            var firstArg = expressionWriter(expression.Arguments[0]);
            var method = expression.Method.Name.ToLowerInvariant();
            //var parameter = expression.Arguments[1] is LambdaExpression ? (expression.Arguments[1] as LambdaExpression).Parameters.First().Name : null;
            string parameter = null;
            var lambdaParameter = expression.Arguments[1] as LambdaExpression;
            if (lambdaParameter != null)
            {
                var first = lambdaParameter.Parameters.First();
                parameter = first.Name ?? first.ToString();
            }
            var predicate = expressionWriter(expression.Arguments[1]);

            return string.Format("{0}/{1}({2}: {3})", firstArg, method, parameter, predicate);
        }

        public string HandleUriFragment(MethodCallExpression expression, Func<Expression, string> expressionWriter, Type sourceType)
        {
            var firstArg = expressionWriter(expression.Arguments[0]);
            var method = expression.Method.Name.ToLowerInvariant();
            //var parameter = expression.Arguments[1] is LambdaExpression ? (expression.Arguments[1] as LambdaExpression).Parameters.First().Name : null;
            string parameter = null;
            var lambdaParameter = expression.Arguments[1] as LambdaExpression;
            if (lambdaParameter != null)
            {
                var first = lambdaParameter.Parameters.First();
                parameter = first.Name ?? first.ToString();
            }
            var predicate = expressionWriter(expression.Arguments[1]);

            return string.Format("{0}/{1}({2}: {3})", firstArg, method, parameter, predicate);
        }
    }
}
