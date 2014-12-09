using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data
{
    public abstract class DataObject<T>
    {
        protected static Dictionary<string, DataProperty> properties = null;
        protected static string label = null;
        protected static Type type = null;
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
        [DataExempt]
        public static Dictionary<string, DataProperty> Properties
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
            var rawProperties = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var property in rawProperties)
            {
                if (property.GetCustomAttributes(typeof(DataExempt),true).Length == 0)
                {
                    properties.Add(property.Name, new DataProperty(type, property));
                }
            }
        }
    }
}
