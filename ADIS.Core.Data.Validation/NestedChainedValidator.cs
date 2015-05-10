using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Validation
{
    public class NestedChainedValidator<T> : IChainedValidationRule
    {
        Validator<T> validator;
        public NestedChainedValidator(Validator<T> validator) {
            this.validator = validator;
        }
        public List<ValidationFailure> Validate(ValidationContext context)
        {
            if (context.PropertyValue != null)
                return this.validator.Validate((T)context.PropertyValue).Errors;
            return null;
        }

    }
}
