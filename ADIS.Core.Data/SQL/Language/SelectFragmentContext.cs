using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.SQL
{
    public class SelectFragmentContext : FragmentContext
    {
        protected ConditionSet whereClause;
        protected SelectFragment selectClause;
        protected TableFragment tableClause;
        protected List<JoinFragment> joins;
        public SelectFragmentContext()
        {
            selectClause = new SelectFragment();
            whereClause = new ConditionSet();
            joins = new List<JoinFragment>();
        }

        internal TableFragment Table
        {
            get
            {
                return tableClause;
            }

        }
        internal ConditionSet Conditions
        {
            get
            {
                return whereClause;
            }
        }



        public void AddInnerJoin(string schema, string name, ConditionSet conditions)
        {
            TableFragment table = new TableFragment(schema, name, this);
            InnerJoinFragment fragment = new InnerJoinFragment(table,conditions);
            this.joins.Add(fragment);
        }
        public void AddJoin(JoinFragment fragment)
        {
            this.joins.Add(fragment);
        }

        public void SetWhere(ConditionSet conditions)
        {
            this.whereClause = conditions;
        }

        public void AddColumn(string tableAlias, string name)
        {
            selectClause.AddColumn(tableAlias, name);
            
        }

        public string SetTable(string schema, string name)
        {
            this.tableClause = new TableFragment(schema, name, this);
            return this.tableClause.Alias;
        }

        public override string ToString()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT ");
            query.Append(selectClause.ToSQL(this));
            query.Append(" FROM ");
            query.Append(tableClause.ToSQL(this));
            if (joins.Count > 0)
            {
                query.Append(" ");
                foreach (var join in joins)
                {
                    query.Append(join.ToSQL(this));
                }
            }
            query.Append(" WHERE ");
            query.Append(whereClause.ToSQL(this));
            return query.ToString();
        }
    }
}
