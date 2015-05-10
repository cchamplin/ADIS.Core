using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Validation
{
    public abstract class ChainedValidationRule : IChainedSingleValidationRule
    {
        private string message;
        public ChainedValidationRule(string message) {
            this.message = message;
        }
        public string Message {
            get{
                return this.message;
            }
            set {
                this.message = value;
            }
        }
        public abstract bool IsValid(ValidationContext context, object propertyValue);
        public List<ValidationFailure> Validate(ValidationContext context)
        {
            if (!IsValid(context,context.PropertyValue)) {
                var results = new List<ValidationFailure>();
                results.Add(new ValidationFailure(context,this.message, this ));
                return results;
            }
            return null;
        }

      
    }
}
