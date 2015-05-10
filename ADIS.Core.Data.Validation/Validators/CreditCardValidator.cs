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
    public static class CreditCardValidator
    {
        public static IRuleChain<T, TProperty> CreditCard<T, TProperty>(this IRuleChain<T, TProperty> chain)
        {
            return chain.ChainRule(new CreditCardRule());
        }
        
         
         public class CreditCardRule : ChainedValidationRule
         {

             public CreditCardRule() : base("Value of {PropertyName} \"{PropertyValue}\" is not a valid credit card number")
             {
                 
             }

           


             public override bool IsValid(ValidationContext context, object propertyValue)
             {
                 if (propertyValue == null)
                     return false;

                 if (propertyValue is string)
                 {
                     string val = propertyValue as string;
                     val = val.Replace("-", "").Replace(" ", "");

                     int checksum = 0;
                     bool evenDigit = false;
                     // http://www.beachnet.com/~hstiles/cardtype.html
                     foreach (char digit in val.ToCharArray().Reverse())
                     {
                         if (!char.IsDigit(digit))
                         {
                             return false;
                         }

                         int digitValue = (digit - '0') * (evenDigit ? 2 : 1);
                         evenDigit = !evenDigit;

                         while (digitValue > 0)
                         {
                             checksum += digitValue % 10;
                             digitValue /= 10;
                         }
                     }

                     return (checksum % 10) == 0;
                 }
                 return false;
             }
         }
    }
}
