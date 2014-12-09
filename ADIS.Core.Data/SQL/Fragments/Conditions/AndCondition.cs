using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.SQL
{
    public class AndCondition : Conditional
    {
        protected Conditional condition;
        public AndCondition(Conditional condition)
        {
            this.condition = condition;
        }
        public override string ToSQL(FragmentContext context)
        {
            return condition.ToSQL(context);
        }
    }
}
