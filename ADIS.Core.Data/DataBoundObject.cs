using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data
{
    public abstract class DataBoundObject<T> : DataObject<T>, IDataBound
    {
        
        protected static string tableName = null;
        protected static string schema = null;
        protected static DataBoundProperty primaryKey = null;

        protected DateTime changeDate;
        protected DateTime addDate;
        protected Guid changeID;
        protected Guid addID;
        
        public DataBoundObject() : base()
        {

            if (tableName == null)
            {
                foreach (Attribute attr in type.GetCustomAttributes(false))
                {
                    if (attr is DataTable)
                    {
                        tableName = ((DataTable)attr).TableName;
                        schema = ((DataTable)attr).Schema;
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
            propertyList = new List<DataProperty>();
            var rawProperties = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            bool added;
            foreach (var property in rawProperties)
            {
                if (property.GetCustomAttributes(typeof(DataExempt), true).Length == 0)
                {
                    added = false;
                    foreach (Attribute attr in property.GetCustomAttributes(false))
                    {
                        if (attr is DataMember)
                        {
                            var prop = new DataBoundProperty(type, property);
                            if (((DataMember)attr).PrimaryKey)
                            {
                                primaryKey = prop;
                            }
                            properties.Add(property.Name, prop);
                            propertyList.Add(prop);
                            added = true;
                            continue;
                        }
                        else if (attr is OneToMany || attr is ManyToMany || attr is ManyToOne)
                        {
                            var prop = new DataBoundProperty(type, property);
                            properties.Add(property.Name, prop);
                            propertyList.Add(prop);
                            added = true;
                            continue;
                        }
                    }

                    if (!added)
                    {
                        var prop = new DataProperty(type, property);
                        properties.Add(property.Name, prop);
                        propertyList.Add(prop);
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
        public DataBoundProperty PrimaryKey
        {
            get { return primaryKey; }
        }

        [DataMember(columnName:"CHANGE_DATE_TIME")]
        [PropertyLabel("Change Date")]
        public DateTime ChangeDateTime
        {
            get
            {
                return changeDate;
            }
            set
            {
                changeDate = value;
            }
        }
        [DataMember(columnName: "CHANGE_ID")]
        [PropertyLabel("Change ID")]
        public Guid ChangeID
        {
            get
            {
                return changeID;
            }
            set
            {
                changeID = value;
            }
        }
        [DataMember(columnName: "ADD_DATE_TIME")]
        [PropertyLabel("Add Date")]
        public DateTime AddDateTime
        {
            get
            {
                return addDate;
            }
            set
            {
                addDate = value;
            }
        }
        [DataMember(columnName: "ADD_ID")]
        [PropertyLabel("Add ID")]
        public Guid AddID
        {
            get
            {
                return addID;
            }
            set
            {
                addID = value;
            }
        }
    }
}
