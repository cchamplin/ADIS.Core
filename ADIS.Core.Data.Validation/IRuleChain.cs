using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Validation
{
    public interface IRuleChain<T,TProperty>
    {
        IRuleChain<T, TProperty> ChainRule(IChainedValidationRule rule);
        IRuleChain<T, TProperty> AddValidator(Validator<TProperty> validator);
        IRuleChain<T, TProperty> AdjustChain(Func<IChainedValidationRule, IChainedValidationRule> func);
        IRuleChain<T, TProperty> AdjustRule(Func<IValidationRule, IChainedValidationRule> func);
    }
}
