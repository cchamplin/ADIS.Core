using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Validation
{
    public static class ContinuationConfigurator
    {
        public static IRuleChain<T, TProperty> Continuation<T, TProperty>(this IRuleChain<T, TProperty> chain, ContinuationStyle style)
        {
            return chain.AdjustRule(x => {

                x.SetContinuation(style);
                return null; 
            });
        }
        public static IRuleChain<T, TProperty> Continuation<T, TProperty>(this IRuleChain<T, TProperty> chain, Expression<Func<IValidationRule, ContinuationStyle>> expression)
        {
            var compiled = expression.Compile();
            return chain.AdjustRule(x =>
            {
                x.SetContinuation(compiled(x));
                return null; 
            });
        }
    }
}
