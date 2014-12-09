using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.SQL
{
    public class SelectFragment : AbstractFragment
    {
        public List<string> columns;

        public SelectFragment()
        {
            columns = new List<string>();
        }

        public void AddColumn(string tableAlias, string columnName)
        {
            columns.Add(tableAlias + "." + columnName);
        }

        public override string ToSQL(FragmentContext context)
        {
            if (columns.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(columns[0]);
                foreach (var column in columns)
                {
                    sb.Append(",");
                    sb.Append(column);
                }
                return sb.ToString();
            }
            return "*";
        }
    }
}
