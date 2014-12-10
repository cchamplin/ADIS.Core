﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data.LINQ.Writers.Interfaces;

namespace ADIS.Core.Data.LINQ.Writers
{
    internal class StringContainsMethodWriter : IMethodCallWriter
    {
        public bool CanHandle(MethodCallExpression expression)
        {
            return expression.Method.DeclaringType == typeof(string)
                       && expression.Method.Name == "Contains";
        }


        public string HandleQueryFragment(MethodCallExpression expression, Func<Expression, string> expressionWriter, Type sourceType)
        {
            var argumentExpression = expression.Arguments[0];
            var obj = expression.Object;

            return string.Format(
                    "substringof({0}, {1})",
                    expressionWriter(argumentExpression),
                    expressionWriter(obj));
        }

        public string HandleUriFragment(MethodCallExpression expression, Func<Expression, string> expressionWriter, Type sourceType)
        {
            var argumentExpression = expression.Arguments[0];
            var obj = expression.Object;

            return string.Format(
                    "substringof({0}, {1})",
                    expressionWriter(argumentExpression),
                    expressionWriter(obj));
        }
    }
}
