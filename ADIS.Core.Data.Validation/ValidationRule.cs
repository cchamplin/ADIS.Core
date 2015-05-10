using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Validation
{
    public class ValidationRule : IValidationRule
    {
        private MemberInfo member;
        private Func<object, object> propertyFunc;
        private LambdaExpression expression;
        private IChainedValidationRule current;
        private List<IChainedValidationRule> rules;
        private ContinuationStyle continuationStyle;
        private ValidationRule(MemberInfo member, Func<object, object> func, LambdaExpression expression)
        {
            this.member = member;
            this.propertyFunc = func;
            this.expression = expression;
            this.continuationStyle = ContinuationStyle.Continue;
            rules = new List<IChainedValidationRule>();
        }
        public static ValidationRule Create<T, TProperty>(MemberInfo member, Func<object, object> func, Expression<Func<T, TProperty>> expression)
        {
            return new ValidationRule(member, func, expression);
        }
        public IChainedValidationRule CurrentRule
        {
            get
            {
                return this.current;
            }
        }
        public void AddRule(IChainedValidationRule rule)
        {
            current = rule;
            rules.Add(rule);
        }
        public void SwapRule(IChainedValidationRule original, IChainedValidationRule replacement)
        {
            for (int x = 0; x < rules.Count; x++)
            {
                if (rules[x] == original)
                {
                    rules[x] = replacement;
                    break;
                }
            }
        }
        public List<ValidationFailure> Validate(object instance)
        {
            bool failed = false;
            List<ValidationFailure> failures = new List<ValidationFailure>();
            var val = propertyFunc(instance);
            var context = new ValidationContext(instance, this.member.Name, val);
            foreach (var validator in rules)
            {
                
                bool validatorFailed = false;
                var results = validator.Validate(context);
                if (results != null)
                {
                    foreach (var result in results)
                    {
                        validatorFailed = true;
                        failed = true;
                        failures.Add(result);
                    }
                }
                if (validatorFailed && continuationStyle == ContinuationStyle.Stop)
                {
                    break;
                }
            }
            return failures;
        }




        public void SetContinuation(ContinuationStyle style)
        {
            this.continuationStyle = style;
        }


        public void RemoveRule(IChainedValidationRule original)
        {
            rules.Remove(original);
        }
    }
}
