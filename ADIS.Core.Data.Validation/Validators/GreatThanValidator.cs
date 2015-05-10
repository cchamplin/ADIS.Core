using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Validation
{
    public static class GreaterThanValidator
    {
        public static IRuleChain<T, TProperty> GreaterThan<T, TProperty>(this IRuleChain<T, TProperty> chain, TProperty comparable)
        {
            return chain.ChainRule(new GreaterThanRule(comparable));
        }
        public static IRuleChain<T, TProperty> GreaterThan<T, TProperty>(this IRuleChain<T, TProperty> chain, Expression<Func<T, TProperty>> expression)
        {
            var compiled = expression.Compile();
            return chain.ChainRule(new GreaterThanRule(x => compiled((T)x)));
        }
        public static IRuleChain<T, TProperty> Greater<T, TProperty>(this IRuleChain<T, TProperty> chain, TProperty comparable)
        {
            return chain.ChainRule(new GreaterThanRule(comparable));
        }
        public static IRuleChain<T, TProperty> Greater<T, TProperty>(this IRuleChain<T, TProperty> chain, Expression<Func<T, TProperty>> expression)
        {
            var compiled = expression.Compile();
            return chain.ChainRule(new GreaterThanRule(x => compiled((T)x)));
        }
        public class GreaterThanRule : ChainedValidationRule
        {
            private Func<object, object> func;
            private object value;

            public GreaterThanRule(object comparable)
                : base("Value of {PropertyName} \"{PropertyValue}\" is less than or equal to \"{Value}\"")
            {
                this.value = comparable;
            }
            public GreaterThanRule(Func<object, object> comparable)
                : base("Value of {PropertyName} \"{PropertyValue}\" is less than or equal to \"{Value}\"")
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
                return ((IComparable)propertyValue).CompareTo(value) > 0;

            }
        }
    }
}
