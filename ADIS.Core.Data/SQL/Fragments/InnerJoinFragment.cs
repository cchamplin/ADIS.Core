using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.SQL
{
    public class InnerJoinFragment : JoinFragment
    {
        public InnerJoinFragment(TableFragment table) : base(table) { }
        public InnerJoinFragment(TableFragment table,ConditionSet conditions) : base(table,conditions) { }
        public override string ToSQL(FragmentContext context)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INNER JOIN ");
            sb.Append(rightTable.ToSQL(context));
            sb.Append(" ON ");
            sb.Append(joinConditions.ToSQL(context));
            return sb.ToString();

        }
    }
}
