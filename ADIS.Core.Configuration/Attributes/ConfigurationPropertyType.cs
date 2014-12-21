using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Configuration
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class ConfigurationPropertyType : Attribute
    {
        protected SqlDbType type;
        public ConfigurationPropertyType(SqlDbType type)
        {
            this.type = type;
        }
        public SqlDbType Type
        {
            get
            {
                return type;
            }
        }
    }
}
