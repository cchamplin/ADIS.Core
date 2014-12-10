using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.SQL
{
    public class TableFragment : AbstractFragment
    {
        protected string schema;
        protected string name;
        protected string alias;
        public TableFragment(string schema, string name, FragmentContext context)
        {
            this.schema = schema;
            this.name = name;
            this.alias = context.NextTable();
        }

        public string Schema
        {
            get { return schema; }
        }
        public string Name
        {
            get { return name; }
        }
        public string Alias
        {
            get
            {
                return alias;
            }
        }


        public override string ToSQL(FragmentContext context)
        {
            return schema + "." + name + " AS " + alias;
        }
    }
}
