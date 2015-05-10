using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Validation
{
    public static class GreaterThanOrEqualsValidator
    {
        public static IRuleChain<T, TProperty> GreaterThanOrEquals<T, TProperty>(this IRuleChain<T, TProperty> chain, TProperty comparable)
        {
            return chain.ChainRule(new GreaterThanOrEqualsRule(comparable));
        }
        public static IRuleChain<T, TProperty> GreaterThanOrEquals<T, TProperty>(this IRuleChain<T, TProperty> chain, Expression<Func<T, TProperty>> expression)
        {
            var compiled = expression.Compile();
            return chain.ChainRule(new GreaterThanOrEqualsRule(x => compiled((T)x)));
        }
        public static IRuleChain<T, TProperty> GreaterThanEquals<T, TProperty>(this IRuleChain<T, TProperty> chain, TProperty comparable)
        {
            return chain.ChainRule(new GreaterThanOrEqualsRule(comparable));
        }
        public static IRuleChain<T, TProperty> GreaterThanEquals<T, TProperty>(this IRuleChain<T, TProperty> chain, Expression<Func<T, TProperty>> expression)
        {
            var compiled = expression.Compile();
            return chain.ChainRule(new GreaterThanOrEqualsRule(x => compiled((T)x)));
        }
        public static IRuleChain<T, TProperty> GreaterEquals<T, TProperty>(this IRuleChain<T, TProperty> chain, TProperty comparable)
        {
            return chain.ChainRule(new GreaterThanOrEqualsRule(comparable));
        }
        public static IRuleChain<T, TProperty> GreaterEquals<T, TProperty>(this IRuleChain<T, TProperty> chain, Expression<Func<T, TProperty>> expression)
        {
            var compiled = expression.Compile();
            return chain.ChainRule(new GreaterThanOrEqualsRule(x => compiled((T)x)));
        }
         public class GreaterThanOrEqualsRule : ChainedValidationRule
         {
             private Func<object, object> func;
             private object value;

             public GreaterThanOrEqualsRule(object comparable) : base("Value of {PropertyName} \"{PropertyValue}\" is less than \"{Value}\"")
             {
                 this.value = comparable;
             }
             public GreaterThanOrEqualsRule(Func<object, object> comparable)
                 : base("Value of {PropertyName} \"{PropertyValue}\" is less than \"{Value}\"")
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
                 return ((IComparable)propertyValue).CompareTo(value) >= 0;
                
             }
         }
    }
}
