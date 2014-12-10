using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.LINQ.Writers.Interfaces
{
    internal interface IMethodCallWriter
    {
        bool CanHandle(MethodCallExpression expression);

        string HandleQueryFragment(MethodCallExpression expression, Func<Expression, string> expressionWriter, Type sourceType);
        string HandleUriFragment(MethodCallExpression expression, Func<Expression, string> expressionWriter, Type sourceType);
    }

    
}
