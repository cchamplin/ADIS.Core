using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Validation
{
    public static class LessThanOrEqualsValidator
    {
        public static IRuleChain<T, TProperty> LessThanOrEquals<T, TProperty>(this IRuleChain<T, TProperty> chain, TProperty comparable)
        {
            return chain.ChainRule(new LessThanOrEqualsRule(comparable));
        }
        public static IRuleChain<T, TProperty> LessThanOrEquals<T, TProperty>(this IRuleChain<T, TProperty> chain, Expression<Func<T, TProperty>> expression)
        {
            var compiled = expression.Compile();
            return chain.ChainRule(new LessThanOrEqualsRule(x => compiled((T)x)));
        }
        public static IRuleChain<T, TProperty> LessThanEquals<T, TProperty>(this IRuleChain<T, TProperty> chain, TProperty comparable)
        {
            return chain.ChainRule(new LessThanOrEqualsRule(comparable));
        }
        public static IRuleChain<T, TProperty> LessThanEquals<T, TProperty>(this IRuleChain<T, TProperty> chain, Expression<Func<T, TProperty>> expression)
        {
            var compiled = expression.Compile();
            return chain.ChainRule(new LessThanOrEqualsRule(x => compiled((T)x)));
        }
        public static IRuleChain<T, TProperty> LessEquals<T, TProperty>(this IRuleChain<T, TProperty> chain, TProperty comparable)
        {
            return chain.ChainRule(new LessThanOrEqualsRule(comparable));
        }
        public static IRuleChain<T, TProperty> LessEquals<T, TProperty>(this IRuleChain<T, TProperty> chain, Expression<Func<T, TProperty>> expression)
        {
            var compiled = expression.Compile();
            return chain.ChainRule(new LessThanOrEqualsRule(x => compiled((T)x)));
        }
        public class LessThanOrEqualsRule : ChainedValidationRule
        {
            private Func<object, object> func;
            private object value;

            public LessThanOrEqualsRule(object comparable)
                : base("Value of {PropertyName} \"{PropertyValue}\" is greater than \"{Value}\"")
            {
                this.value = comparable;
            }
            public LessThanOrEqualsRule(Func<object, object> comparable)
                : base("Value of {PropertyName} \"{PropertyValue}\" is greater than \"{Value}\"")
            {
                this.func = comparable;
            }
            public object Value
            {
                get
                {
                    return value;
                }
            }
            public override bool IsValid(ValidationContext context, object propertyValue)
            {
                if (func != null)
                {
                    this.value = func(context.Instance);
                }
                return ((IComparable)propertyValue).CompareTo(value) <= 0;

            }
        }
    }
}
