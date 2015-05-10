using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADIS.Core.Data.Validation
{
    public class ValidationFailure
    {
        private string formattedMessage;
        public ValidationFailure(ValidationContext context, string message,object ruleContext)
        {
            this.formattedMessage = message.FormatString(new object[] { context,ruleContext});
        }
        public string FormattedMessage
        {
            get
            {
                return this.formattedMessage;
            }
        }
    }
}
