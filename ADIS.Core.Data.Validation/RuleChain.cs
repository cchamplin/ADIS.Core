using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Validation
{
    public class RuleChain<T,TProperty> : IRuleChain<T,TProperty>
    {
        private ValidationRule rule;
        public RuleChain(ValidationRule rule)
        {
            this.rule = rule;
        }
        public IRuleChain<T, TProperty> AddValidator(Validator<TProperty> validator)
        {
            this.rule.AddRule(new NestedChainedValidator<TProperty>(validator));
            return this;
        }
        public IRuleChain<T, TProperty> ChainRule(IChainedValidationRule chainedRule)
        {
            this.rule.AddRule(chainedRule);
            return this;
        }
        public IRuleChain<T, TProperty> AdjustChain(Func<IChainedValidationRule,IChainedValidationRule> func)
        {
            var result = func(this.rule.CurrentRule);
            if (result != null)
                this.rule.AddRule(result);
            return this;
        }
        public IRuleChain<T, TProperty> AdjustRule(Func<IValidationRule, IChainedValidationRule> func)
        {
            var result = func(this.rule);
            if (result != null)
                this.rule.AddRule(result);
            return this;
        }
    }
}
