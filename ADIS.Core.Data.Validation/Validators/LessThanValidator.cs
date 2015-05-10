using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Validation
{
    public static class LessThanValidator
    {
        public static IRuleChain<T, TProperty> LessThan<T, TProperty>(this IRuleChain<T, TProperty> chain, TProperty comparable)
        {
            return chain.ChainRule(new LessThanRule(comparable));
        }
        public static IRuleChain<T, TProperty> LessThan<T, TProperty>(this IRuleChain<T, TProperty> chain, Expression<Func<T, TProperty>> expression)
        {
            var compiled = expression.Compile();
            return chain.ChainRule(new LessThanRule(x => compiled((T)x)));
        }
        public static IRuleChain<T, TProperty> Less<T, TProperty>(this IRuleChain<T, TProperty> chain, TProperty comparable)
        {
            return chain.ChainRule(new LessThanRule(comparable));
        }
        public static IRuleChain<T, TProperty> Less<T, TProperty>(this IRuleChain<T, TProperty> chain, Expression<Func<T, TProperty>> expression)
        {
            var compiled = expression.Compile();
            return chain.ChainRule(new LessThanRule(x => compiled((T)x)));
        }
        public class LessThanRule : ChainedValidationRule
        {
            private Func<object, object> func;
            private object value;

            public LessThanRule(object comparable)
                : base("Value of {PropertyName} \"{PropertyValue}\" is greater than or equal to \"{Value}\"")
            {
                this.value = comparable;
            }
            public LessThanRule(Func<object, object> comparable)
                : base("Value of {PropertyName} \"{PropertyValue}\" is greater than or equal to \"{Value}\"")
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
                return ((IComparable)propertyValue).CompareTo(value) < 0;

            }
        }
    }
}
