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
        protected string alias;
        public ConfigurationProperty(string alias)
        {
            this.alias = alias;
        }
        public string Alias
        {
            get
            {
                return alias;
            }
        }
    }
}
