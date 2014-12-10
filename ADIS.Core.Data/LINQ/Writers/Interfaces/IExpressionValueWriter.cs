using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.LINQ.Writers.Interfaces
{
    internal interface IExpressionValueWriter
    {
        bool CanHandle(Expression expression);

        string HandleQueryFragment(Expression expression, ParameterExpression rootParameter, INameResolver memberNameResolver, Func<Expression, string> expressionWriter, Type sourceType);
        string HandleUriFragment(Expression expression, ParameterExpression rootParameter, INameResolver memberNameResolver, Func<Expression, string> expressionWriter, Type sourceType);
    }
}
