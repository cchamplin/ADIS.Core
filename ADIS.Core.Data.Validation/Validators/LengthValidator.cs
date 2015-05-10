using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Validation
{
    public static class LengthValidator
    {
        public static IRuleChain<T, TProperty> IsEmpty<T, TProperty>(this IRuleChain<T, TProperty> chain,int min, int max)
        {
            return chain.ChainRule(new LengthRule(min,max));
        }
        public static IRuleChain<T, TProperty> Empty<T, TProperty>(this IRuleChain<T, TProperty> chain, int min, int max)
        {
            return chain.ChainRule(new LengthRule(min,max));
        }
         
         public class LengthRule : ChainedValidationRule
         {

             private int min;
             private int max;
             public LengthRule(int min, int max) : base("Value of {PropertyName} \"{PropertyValue}\" has an invalid length")
             {
                 this.min = min;
                 this.max = max;
             }


             public override bool IsValid(ValidationContext context, object propertyValue)
             {
                 if (propertyValue == null)
                     return true;
                 if (propertyValue is string)
                 {
                     if (((string)propertyValue).Length >= min && ((string)propertyValue).Length <= max)
                         return true;
                     return false;
                 }
                 return true;
                
             }
         }
    }
}
