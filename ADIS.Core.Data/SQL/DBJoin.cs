using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.SQL
{
    public class DBJoin
    {
        protected JoinFragment fragment;
        protected FragmentContext context;
        public DBJoin(JoinFragment fragment, FragmentContext context)
        {
            this.fragment = fragment;
            this.context = context;
        }
        public string GetTableAlias()
        {
            return fragment.Alias;
        }
        public ConditionSet Conditions
        {
            get
            {
                return fragment.Conditions;
            }
        }
    }
}
