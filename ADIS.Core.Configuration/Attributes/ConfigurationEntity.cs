using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Configuration
{
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    public class ConfigurationEntity : Attribute
    {
        protected ConfigurationEntityType type;
        protected string name;
        protected string tableName;
        public ConfigurationEntity(ConfigurationEntityType type, string name, string tableName = null)
        {
            this.type = type;
            this.name = name;
            if (this.type == ConfigurationEntityType.DataBound && tableName == null)
            {
                throw new Exception("Data bound configuratio entities must provide a table name");
            }
            this.tableName = tableName;
        }
        public ConfigurationEntityType Type
        {
            get
            {
                return type;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public string TableName
        {
            get
            {
                return tableName;
            }
        }
    }
}
