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
        public string tableName;
        public string schema;
        public string primaryKey;
        public DataTable(string name)
        {
            this.tableName = name;
        }
        public DataTable(string schema, string name)
        {
            this.schema = schema;
            this.tableName = name;
        }
        public DataTable(string schema, string name, string primaryKey)
        {
            this.schema = schema;
            this.tableName = name;
            this.primaryKey = primaryKey;
        }
    }
}
