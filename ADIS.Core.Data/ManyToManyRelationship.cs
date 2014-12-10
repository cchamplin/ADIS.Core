using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data
{
    public class ManyToManyRelationship : DataRelationship
    {
        protected string schema;
        protected string table;
        protected string fkColumn;
        public ManyToManyRelationship(string schema, string table, string fkColumn, string keyColumn)
            : base(keyColumn)
        {
            this.schema = schema;
            this.table = table;
            this.fkColumn = fkColumn;
            this.keyColumn = keyColumn;
        }
    }
}
