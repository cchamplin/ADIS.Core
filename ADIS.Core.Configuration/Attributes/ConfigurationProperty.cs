using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Configuration
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class ConfigurationProperty : Attribute
    {
        protected string column;
        public ConfigurationProperty(string columnName)
        {
            this.column = columnName;
        }
        public string Column
        {
            get
            {
                return column;
            }
        }
    }
}
