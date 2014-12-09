using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data.SQL;

namespace ADIS.Core.Data.Providers
{
    public class MSSQL : IDataProvider
    {
        public MSSQL()
        {
        }
        public T FetchObject<T>(DataBoundObject<T> dbo, object keyValue, string op = "=") where T : DataBoundObject<T>
        {
            if (dbo.PrimaryKey == null)
            {
                throw new Exception("DataBoundObject does not identify a primary key");
            }
            var query = new SelectFragmentContext();
            var table = query.SetTable(dbo.Schema, dbo.TableName);
            ConditionSet cs = new ConditionSet();
            var condition = new PropertyCondition();
            condition.SetPropertyLeft(table, dbo.BoundProperties[dbo.PrimaryKey]);
            condition.SetRight(keyValue);
            condition.Operation = op;
            cs.Conditions.Add(condition);
            query.SetWhere(cs);
            System.Diagnostics.Debug.WriteLine(query.ToString());
            return default(T);
        }

        public DBSelect<T> QueryObject<T>(DataBoundObject<T> dbo) where T : DataBoundObject<T>
        {

            var select = new DBSelect<T>(dbo.Schema,dbo.TableName);
            return select;
        }
    }
}
