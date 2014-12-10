using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.SQL
{
    public class ConditionSet : Conditional
    {

        protected List<Conditional> Conditions;
        protected string separator;
        public ConditionSet(string separator)
        {
            this.separator = separator;
            Conditions = new List<Conditional>();
        }
        public Conditional Condition(Conditional condition)
        {
            Conditions.Add(condition);
            return this;
        }
        public Conditional Condition(string literalLeft, string literalRight, string op = "=")
        {
            Conditions.Add(new PropertyCondition());
            return this;
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

                sql.Append(" ");
                sql.Append(this.separator);
                sql.Append(" ");
                sql.Append(Conditions[x].ToSQL(context));
            }
            sql.Append(")");
            return sql.ToString();
            
        }
    }
}
