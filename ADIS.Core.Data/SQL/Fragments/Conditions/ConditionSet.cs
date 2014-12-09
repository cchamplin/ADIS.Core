using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.SQL
{
    public class ConditionSet : Conditional
    {

        public List<Conditional> Conditions;
        public ConditionSet()
        {
            Conditions = new List<Conditional>();
        }
        public void AddCondition(Conditional condition)
        {
            Conditions.Add(condition);
        }


        public override string ToSQL(FragmentContext context)
        {
            if (Conditions.Count == 0)
                return "";
            if (Conditions.Count == 1)
                return "(" + Conditions[0].ToSQL(context) + ")";

            StringBuilder sql = new StringBuilder();
            sql.Append("(");
            sql.Append(Conditions[0].ToSQL(context));
            for (int x = 1; x < Conditions.Count; x++)
            {
                if (Conditions[x] is AndCondition)
                {
                    sql.Append(" AND ");
                   
                }
                else if (Conditions[x] is OrCondition)
                {
                    sql.Append(" OR ");
                }
                else
                {
                    sql.Append(" AND ");
                }
                
                sql.Append(Conditions[x].ToSQL(context));
            }
            sql.Append(")");
            return sql.ToString();
            
        }
    }
}
