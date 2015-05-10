using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Validation
{
    public static class NotEmptyValidator
    {
        public static IRuleChain<T, TProperty> IsNotEmpty<T, TProperty>(this IRuleChain<T, TProperty> chain)
        {
            return chain.ChainRule(new NotEmptyRule());
        }
        public static IRuleChain<T, TProperty> NotEmpty<T, TProperty>(this IRuleChain<T, TProperty> chain)
        {
            return chain.ChainRule(new NotEmptyRule());
        }

        public class NotEmptyRule : ChainedValidationRule
        {


            public NotEmptyRule()
                : base("Value of {PropertyName} \"{PropertyValue}\" is empty")
            {

            }


            public override bool IsValid(ValidationContext context, object propertyValue)
            {
                if (propertyValue == null)
                    return false;

                if (propertyValue is string)
                    return propertyValue != null && !string.IsNullOrWhiteSpace(propertyValue as string);

                if (propertyValue is int)
                    return (int)propertyValue != 0;
                if (propertyValue is short)
                    return (short)propertyValue != 0;
                if (propertyValue is long)
                    return (long)propertyValue != 0;
                if (propertyValue is Int32)
                    return (Int32)propertyValue != 0;
                if (propertyValue is Int16)
                    return (Int16)propertyValue != 0;
                if (propertyValue is Int64)
                    return (Int64)propertyValue != 0;
                if (propertyValue is bool)
                    return (bool)propertyValue != false;
                if (propertyValue is double)
                    return (double)propertyValue != 0;
                if (propertyValue is Double)
                    return (Double)propertyValue != 0;
                if (propertyValue is float)
                    return (double)propertyValue != 0;
                if (propertyValue is decimal)
                    return (decimal)propertyValue != 0;
                if (propertyValue is Decimal)
                    return (Decimal)propertyValue != 0;
                if (propertyValue is Array)
                {
                    return (propertyValue as Array).Length != 0;
                }
                return true;
            }
        }
    }
}
