using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data
{
    public abstract class DataBoundObject<T> : DataObject<T>
    {
        
        protected static string tableName = null;
        protected static string schema = null;
        protected static string primaryKey = null;
        public DataBoundObject() : base()
        {

            if (tableName == null)
            {
                foreach (Attribute attr in type.GetCustomAttributes(false))
                {
                    if (attr is DataTable)
                    {
                        tableName = ((DataTable)attr).tableName;
                        schema = ((DataTable)attr).schema;
                        primaryKey = ((DataTable)attr).primaryKey;
                    }
                }
            }
            // TODO type this exception;
            if (tableName == null)
            {
                throw new Exception("Data bound properties must specify DataTable attribute");
            }
           
        }

        protected override void ScanProperties()
        {
            properties = new Dictionary<string, DataProperty>();
            var rawProperties = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            bool added;
            foreach (var property in rawProperties)
            {
                if (property.GetCustomAttributes(typeof(DataExempt), true).Length == 0)
                {
                    added = false;
                    foreach (Attribute attr in property.GetCustomAttributes(false))
                    {
                        if (attr is PropertyLabel)
                        {
                            properties.Add(property.Name, new DataBoundProperty(type, property));
                            added = true;
                            continue;
                        }
                    }

                    if (!added)
                    {
                        properties.Add(property.Name, new DataProperty(type, property));
                    }
                }
            }
        }

        [DataExempt]
        public Dictionary<string, DataProperty> BoundProperties
        {
            get
            {
                if (properties == null)
                {
                    ScanProperties();
                }
                return properties;
            }
        }

        [DataExempt]
        public string TableName
        {
            get { return tableName; }
        }
        [DataExempt]
        public string Schema
        {
            get { return schema; }
        }

        [DataExempt]
        public string PrimaryKey
        {
            get { return primaryKey; }
        }
    }
}
