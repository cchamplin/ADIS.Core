using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADIS.Core.Data.Validation
{
    public class ValidationResult
    {
        private List<ValidationFailure> failures;
        private bool failed;
        public ValidationResult(List<ValidationFailure> failures)
        {
            this.failures = failures;
            if (this.failures == null || this.failures.Count == 0)
                this.failed = false;
            else
                this.failed = true;
        }
        public bool IsValid
        {
            get
            {
                return !this.failed;
            }
        }
        public List<ValidationFailure> Errors
        {
            get
            {
                return this.failures;
            }

        }
    }
}
