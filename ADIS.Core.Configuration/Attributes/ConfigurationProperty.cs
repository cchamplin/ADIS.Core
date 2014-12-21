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
        protected int dataLength;
        protected bool primary;
        protected bool nullable;



        public ConfigurationProperty(string columnName,int dataLength = -1)
        {
            this.column = columnName;
            this.dataLength = dataLength;
            this.primary = false;
            this.nullable = false;
        }

        public ConfigurationProperty(string columnName, bool primary, int dataLength = -1)
        {
            this.column = columnName;
            this.primary = primary;
            this.dataLength = dataLength;
        }

        public ConfigurationProperty(string columnName, bool primary, bool nullable, int dataLength = -1)
        {
            this.column = columnName;
            this.primary = primary;
            this.nullable = nullable;
            this.dataLength = dataLength;
        }

        public string Column
        {
            get
            {
                return column;
            }
        }
        public bool Primary
        {
            get
            {
                return primary;
            }
        }
        public bool Nullable
        {
            get
            {
                return nullable;
            }
        }
        public int Length
        {
            get
            {
                return dataLength;
            }
        }
    }
}
