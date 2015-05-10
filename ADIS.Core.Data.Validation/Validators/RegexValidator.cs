using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Validation
{
    public static class RegexValidator
    {
        public static IRuleChain<T, TProperty> Regex<T, TProperty>(this IRuleChain<T, TProperty> chain, string pattern)
        {
            return chain.ChainRule(new RegexRule(pattern));
        }
        


         
         public class RegexRule : ChainedValidationRule
         {

             private string pattern;
             public RegexRule(string pattern) : base("Value of {PropertyName} \"{PropertyValue}\" is not in a valid format")
             {
                 
             }

             public override bool IsValid(ValidationContext context, object propertyValue)
             {
                 if (propertyValue == null)
                     return true;

                 if (propertyValue is string)
                 {
                     return (System.Text.RegularExpressions.Regex.IsMatch(propertyValue as string,pattern));
                 }
                 return true;
             }
         }
    }
}
