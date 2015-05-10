using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Validation
{
    public static class IsNullValidator
    {
        public static IRuleChain<T, TProperty> IsNull<T, TProperty>(this IRuleChain<T, TProperty> chain)
        {
            return chain.ChainRule(new IsNullRule());
        }
        public static IRuleChain<T, TProperty> Null<T, TProperty>(this IRuleChain<T, TProperty> chain)
        {
            return chain.ChainRule(new IsNullRule());
        }

        public class IsNullRule : ChainedValidationRule
        {


            public IsNullRule()
                : base("Value of {PropertyName} \"{PropertyValue}\" is not null")
            {

            }


            public override bool IsValid(ValidationContext context, object propertyValue)
            {
                if (propertyValue == null)
                    return true;
                return false;
            }
        }
    }
}
