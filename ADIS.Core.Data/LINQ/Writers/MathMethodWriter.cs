using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ADIS.Core.Data.LINQ.Writers.Interfaces;

namespace ADIS.Core.Data.LINQ.Writers
{
    internal abstract class MathMethodWriter : IMethodCallWriter
    {
        protected abstract string MethodName { get; }

        public abstract bool CanHandle(MethodCallExpression expression);


        public string HandleQueryFragment(MethodCallExpression expression, Func<Expression, string> expressionWriter, Type sourceType)
        {
            var mathArgument = expression.Arguments[0];

            return string.Format("{0}({1})", MethodName, expressionWriter(mathArgument));
        }

        public string HandleUriFragment(MethodCallExpression expression, Func<Expression, string> expressionWriter, Type sourceType)
        {
            var mathArgument = expression.Arguments[0];

            return string.Format("{0}({1})", MethodName, expressionWriter(mathArgument));
        }
    }
}
