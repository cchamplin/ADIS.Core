using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Validation
{
    public static class NotEqualsValidator
    {
        public static IRuleChain<T, TProperty> IsNotEqual<T, TProperty>(this IRuleChain<T, TProperty> chain, TProperty comparable)
        {
            return chain.ChainRule(new NotEqualsRule(comparable));
        }
         public static IRuleChain<T, TProperty> IsNotEqual<T, TProperty>(this IRuleChain<T, TProperty> chain, Expression<Func<T,TProperty>> expression)
        {
            var compiled = expression.Compile();
            return chain.ChainRule(new NotEqualsRule(x => compiled((T)x)));
        }
         public static IRuleChain<T, TProperty> NotEqual<T, TProperty>(this IRuleChain<T, TProperty> chain, TProperty comparable)
         {
             return chain.ChainRule(new NotEqualsRule(comparable));
         }
         public static IRuleChain<T, TProperty> NotEqual<T, TProperty>(this IRuleChain<T, TProperty> chain, Expression<Func<T, TProperty>> expression)
         {
             var compiled = expression.Compile();
             return chain.ChainRule(new NotEqualsRule(x => compiled((T)x)));
         }
         public class NotEqualsRule : ChainedValidationRule
         {
             private Func<object, object> func;
             private object value;

             public NotEqualsRule(object comparable) : base("Value of {PropertyName} \"{PropertyValue}\" is equal to \"{Value}\"")
             {
                 this.value = comparable;
             }
             public NotEqualsRule(Func<object, object> comparable)
                 : base("Value of {PropertyName} \"{PropertyValue}\" is equal to \"{Value}\"")
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
                 if (propertyValue == null)
                     return value != null;
                 return !propertyValue.Equals(value);
             }
         }
    }
}
