using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Validation
{
    public static class EqualsValidator
    {
        public static IRuleChain<T, TProperty> IsEqual<T, TProperty>(this IRuleChain<T, TProperty> chain, TProperty comparable)
        {
            return chain.ChainRule(new EqualsRule(comparable));
        }
         public static IRuleChain<T, TProperty> IsEqual<T, TProperty>(this IRuleChain<T, TProperty> chain, Expression<Func<T,TProperty>> expression)
        {
            var compiled = expression.Compile();
            return chain.ChainRule(new EqualsRule(x => compiled((T)x)));
        }
         public class EqualsRule : ChainedValidationRule
         {
             private Func<object, object> func;
             private object value;

             public EqualsRule(object comparable) : base("Value of {PropertyName} \"{PropertyValue}\" is not equal to \"{Value}\"")
             {
                 this.value = comparable;
             }
             public EqualsRule(Func<object, object> comparable)
                 : base("Value of {PropertyName} \"{PropertyValue}\" is not equal to \"{Value}\"")
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
                     return value == null;
                 return propertyValue.Equals(value);
             }
         }
    }
}
