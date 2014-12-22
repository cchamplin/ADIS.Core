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
        protected string schema;
        public ConfigurationEntity(ConfigurationEntityType type, string name, string tableName = null, string schema = null)
        {
            this.type = type;
            this.name = name;
            if (this.type == ConfigurationEntityType.DataBound && tableName == null)
            {
                throw new Exception("Data bound configuratio entities must provide a table name");
            }
            this.tableName = tableName;
            if (this.schema == null)
            {
                this.schema = "adis";
            }
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
        public string Schema
        {
            get
            {
                return schema;
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
