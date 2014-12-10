using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class ManyToMany : Attribute
    {
        public string KeyColumn;
        public string Schema;
        public string Table;
        public string FkColumn;
        public ManyToMany(string schema, string table, string keyColumn, string fkColumn)
        {
            this.KeyColumn = keyColumn;
            this.Schema = schema;
            this.Table = table;
            this.FkColumn = fkColumn;
        }
    }
}
