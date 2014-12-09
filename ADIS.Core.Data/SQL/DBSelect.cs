using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.SQL
{
    public class DBSelect<T>
    {
        protected SelectFragmentContext query;
        public DBSelect(string schema, string table)
        {
            query = new SelectFragmentContext();
            query.SetTable(schema, table);
        }

        public string GetTableAlias()
        {
            return query.Table.Alias;
        }
        public DBJoin InnerJoin(string schema, string name)
        {
            TableFragment table = new TableFragment(schema,name,query);
            InnerJoinFragment fragment = new InnerJoinFragment(table);
            query.AddJoin(fragment);
            return new DBJoin(fragment, query);
        }

        public ConditionSet Conditions
        {
            get {
                return query.Conditions;
            }
        }
       
        public T Fetch()
        {
            return Execute();
        }
        public T Execute()
        {
            System.Diagnostics.Debug.WriteLine(query.ToString());
            return default(T);
        }
    }
}
