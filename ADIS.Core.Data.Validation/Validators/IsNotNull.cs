using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Validation
{
    public static class IsNotNullValidator
    {
        public static IRuleChain<T, TProperty> IsNotNull<T, TProperty>(this IRuleChain<T, TProperty> chain)
        {
            return chain.ChainRule(new IsNotNullRule());
        }
        public static IRuleChain<T, TProperty> NotNull<T, TProperty>(this IRuleChain<T, TProperty> chain)
        {
            return chain.ChainRule(new IsNotNullRule());
        }

        public class IsNotNullRule : ChainedValidationRule
        {


            public IsNotNullRule()
                : base("Value of {PropertyName} \"{PropertyValue}\" is null")
            {

            }


            public override bool IsValid(ValidationContext context, object propertyValue)
            {
                if (propertyValue != null)
                    return true;
                return false;
            }
        }
    }
}
