using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Validation
{
    public static class WithMessageConfigurator
    {
        public static IRuleChain<T, TProperty> WithMessage<T, TProperty>(this IRuleChain<T, TProperty> chain, string message)
        {
            return chain.AdjustChain(x => { 
                if (x is IChainedSingleValidationRule)
                ((IChainedSingleValidationRule)x).Message = message; 
                return null; 
            });
        }
        public static IRuleChain<T, TProperty> WithMessage<T, TProperty>(this IRuleChain<T, TProperty> chain, Expression<Func<IChainedValidationRule, string>> expression)
        {
            var compiled = expression.Compile();
            return chain.AdjustChain(x =>
            {
                if (x is IChainedSingleValidationRule)
                    ((IChainedSingleValidationRule)x).Message = compiled(x);
                return null; 
            });
        }
    }
}
