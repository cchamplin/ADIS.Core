using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.ComponentServices
{
    public interface IDatabaseProvider
    {
        DbConnection NewConnection(string connectionString);
        DbCommand NewCommand();
        DbCommand NewCommand(string query, DbConnection connection);
        DbCommand NewCommand(DbConnection connection);
        DbParameter NewParameter();
        DbParameter NewParameter(string name, object value);
        DbParameter NewParameter(string name, int value);
        DbParameter NewParameter(string name, DbDataType type);
        DbParameter NewParameter(string name, object value, DbDataType type);
        DbParameter NewParameter(string name, DbDataType type, int size);
    }
}
