using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data
{
    public abstract class DataObject<T> : DataObjectBase
    {
        
        protected static string label = null;
        protected static Type type = null;
        protected static Dictionary<string, DataProperty> properties = null;
        protected static List<DataProperty> propertyList = null;
        public DataObject()
        {
            type = this.GetType();
            if (label == null) {
                label = type.Name;
                foreach (Attribute attr in type.GetCustomAttributes(false))
                {
                    if (attr is ObjectLabel)
                    {
                        label = ((ObjectLabel)attr).label;
                    }
                }
            }
            if (properties == null)
            {
                ScanProperties();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        ///
         public override List<DataProperty> PropertyList
        {
            get
            {
                return propertyList;
            }
        }
         public override Dictionary<String, DataProperty> Properties
         {
             get
             {
                 return properties;
             }
         }
        
        [DataExempt]
        public string Label
        {
            get
            {
                return label;
            }
        }

        protected virtual void ScanProperties()
        {
            properties = new Dictionary<string, DataProperty>();
            propertyList = new List<DataProperty>();
            var rawProperties = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var property in rawProperties)
            {
                if (property.GetCustomAttributes(typeof(DataExempt),true).Length == 0)
                {
                    var prop = new DataProperty(type, property);
                    properties.Add(property.Name, prop);
                    propertyList.Add(prop);
                }
            }
        }
    }
}
