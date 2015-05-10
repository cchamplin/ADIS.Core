using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADIS.Core.Data.Validation
{
   
    public class ValidationContext
    {
        protected object instance;
        protected string propertyName;
        protected object value;
        public ValidationContext(object instance, string propertyName, object value)
        {
            this.instance = instance;
            this.propertyName = propertyName;
            this.value = value;
        }
        public object Instance
        {
            get
            {
                return this.instance;
            }
        }
        public string PropertyName
        {
            get
            {
                return this.propertyName;
            }
        }
        public object PropertyValue
        {
            get
            {
                return this.value;
            }
        }
    }
}
