using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.SQL
{
    public abstract class  JoinFragment : AbstractFragment
    {
        protected TableFragment rightTable;
        protected ConditionSet joinConditions;
         public JoinFragment(TableFragment table)
        {
            this.rightTable = table;
            this.joinConditions = new And();
        }

         public JoinFragment(TableFragment table, ConditionSet conditions)
        {
            this.rightTable = table;
            this.joinConditions = conditions;
        }
        


        public ConditionSet Conditions
        {
            get
            {
                return joinConditions;
            }
        }

        public string Alias
        {
            get
            {
                return rightTable.Alias;
            }
        }
    }
}
