using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data.LINQ.Writers.Interfaces;

namespace ADIS.Core.Data.LINQ.Writers
{
    internal class EmptyAnyMethodWriter : IMethodCallWriter
    {

        private static readonly MethodInfo AnyMethod = typeof(Enumerable).GetMethods()
            .FirstOrDefault(m => m.Name == "Any" && m.GetParameters().Length == 2);

        public bool CanHandle(MethodCallExpression expression)
        {
            return expression.Method.Name == "Any" && expression.Arguments.Count == 1;
        }

        public string HandleQueryFragment(MethodCallExpression expression, Func<Expression, string> expressionWriter, Type sourceType)
        {
            //var firstArg = expressionWriter(expression.Arguments[0]);
            //return string.Format("{0}/any()", firstArg);

            var argumentType = expression.Arguments[0].Type;

            var parameterType = argumentType.IsGenericType
                                    ? argumentType.GetGenericArguments()[0]
                                    : typeof(object);
            var anyMethod = AnyMethod.MakeGenericMethod(parameterType);

            var parameter = Expression.Parameter(parameterType);

            var lambda = Expression.Lambda(Expression.Constant(true), parameter);
            var rewritten = Expression.Call(expression.Object, anyMethod, expression.Arguments[0], lambda);
            return expressionWriter(rewritten);
        }

          public string HandleUriFragment(MethodCallExpression expression, Func<Expression, string> expressionWriter, Type sourceType)
        {
            //var firstArg = expressionWriter(expression.Arguments[0]);
            //return string.Format("{0}/any()", firstArg);

            var argumentType = expression.Arguments[0].Type;

            var parameterType = argumentType.IsGenericType
                                    ? argumentType.GetGenericArguments()[0]
                                    : typeof(object);
            var anyMethod = AnyMethod.MakeGenericMethod(parameterType);

            var parameter = Expression.Parameter(parameterType);

            var lambda = Expression.Lambda(Expression.Constant(true), parameter);
            var rewritten = Expression.Call(expression.Object, anyMethod, expression.Arguments[0], lambda);
            return expressionWriter(rewritten);
        }
    }
}
