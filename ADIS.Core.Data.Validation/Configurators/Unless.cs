using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Validation
{
    public static class UnlessConfigurator
    {
        public static IRuleChain<T, TProperty> Unless<T, TProperty>(this IRuleChain<T, TProperty> chain, Func<T, bool> evaluator)
        {
            return chain.AdjustRule(x =>
            {
                x.SwapRule(x.CurrentRule, new UnlessRule<T>(evaluator, x.CurrentRule as ChainedValidationRule));
                return null;
            });
        }
        public class UnlessRule<T> : IWrappedChainedValidationRule
        {

            IChainedValidationRule wrapped;
            Func<T, bool> evaluator;
            public UnlessRule(Func<T, bool> evaluator, IChainedValidationRule rule)
            {
                this.evaluator = evaluator;
                this.wrapped = rule;
            }

            public string Message
            {
                get
                {
                    if (wrapped is IChainedSingleValidationRule)
                        return ((IChainedSingleValidationRule)wrapped).Message;
                    return null;
                }
                set
                {
                    if (wrapped is IChainedSingleValidationRule)
                        ((IChainedSingleValidationRule)wrapped).Message = value;
                }
            }

            public List<ValidationFailure> Validate(ValidationContext context)
            {
                if (!evaluator((T)context.Instance))
                {
                    return wrapped.Validate(context);
                }
                return null;
            }
        }


    }
}
