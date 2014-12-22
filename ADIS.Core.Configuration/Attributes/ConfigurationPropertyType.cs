using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.ComponentServices;

namespace ADIS.Core.Configuration
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class ConfigurationPropertyType : Attribute
    {
        protected DbDataType type;
        public ConfigurationPropertyType(DbDataType type)
        {
            this.type = type;
        }
        public DbDataType Type
        {
            get
            {
                return type;
            }
        }
    }
}
