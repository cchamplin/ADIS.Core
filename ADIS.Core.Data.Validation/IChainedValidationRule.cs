using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Validation
{
    public interface IChainedValidationRule
    {
        List<ValidationFailure> Validate(ValidationContext context);
        
    }
}
