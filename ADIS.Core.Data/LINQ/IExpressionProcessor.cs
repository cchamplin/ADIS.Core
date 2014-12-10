using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.LINQ
{
    internal interface IExpressionProcessor
    {
        object ProcessMethodCall<T>(MethodCallExpression methodCall, QueryBuilder builder, Func<QueryBuilder, bool, IEnumerable<T>> resultLoader, Func<Type, QueryBuilder, IEnumerable> intermediateResultLoader);
    }
}
