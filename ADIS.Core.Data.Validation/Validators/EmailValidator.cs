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
    public static class EmailValidator
    {
        public static IRuleChain<T, TProperty> Email<T, TProperty>(this IRuleChain<T, TProperty> chain)
        {
            return chain.ChainRule(new EmailRule());
        }
        public static IRuleChain<T, TProperty> Email<T, TProperty>(this IRuleChain<T, TProperty> chain, string domain)
        {
            return chain.ChainRule(new EmailRule(domain));
        }

         public static IRuleChain<T, TProperty> Email<T, TProperty>(this IRuleChain<T, TProperty> chain, string[] domains)
        {
            return chain.ChainRule(new EmailRule(domains));
        }
         
         public class EmailRule : ChainedValidationRule
         {

             private string[] domains;
             private const string emailPattern = "^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$";
             public EmailRule() : base("Value of {PropertyName} \"{PropertyValue}\" is not a valid email address")
             {
                 
             }

            public EmailRule(string domain) : base("Value of {PropertyName} \"{PropertyValue}\" is not a valid email address or has an invalid domain")
             {
                 this.domains = new string[] { domain };
             }
            public EmailRule(string[] domains)
                : base("Value of {PropertyName} \"{PropertyValue}\" is not a valid email address or has an invalid domain")
             {
                 this.domains = domains;
             }


             public override bool IsValid(ValidationContext context, object propertyValue)
             {
                 if (propertyValue == null)
                     return false;

                 if (propertyValue is string)
                 {
                     return (Regex.IsMatch(propertyValue as string,emailPattern));
                 }
                 return false;
             }
         }
    }
}
