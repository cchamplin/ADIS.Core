using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data
{
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    public class DataTable : Attribute
    {
        protected string tableName;
        protected string schema;
        public DataTable(string name)
        {
            this.tableName = name;
            this.schema = null;
        }
        public DataTable(string schema, string name)
        {
            this.schema = schema;
            this.tableName = name;
        }
        public string TableName
        {
            get
            {
                return tableName;
            }
        }
        public string Schema
        {
            get
            {
                return schema;
            }
        }
    }
}
