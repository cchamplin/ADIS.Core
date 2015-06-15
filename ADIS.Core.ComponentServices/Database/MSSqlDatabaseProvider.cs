using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.ComponentServices
{
    public class MSSqlDatabaseProvider : IDatabaseProvider
    {
        public System.Data.Common.DbConnection NewConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        public System.Data.Common.DbCommand NewCommand()
        {
            return new SqlCommand();
        }

        public System.Data.Common.DbCommand NewCommand(string query, System.Data.Common.DbConnection connection)
        {
            return new SqlCommand(query, (SqlConnection)connection);
        }

        public System.Data.Common.DbCommand NewCommand(System.Data.Common.DbConnection connection)
        {
            var command = new SqlCommand();
            command.Connection = (SqlConnection)connection;
            return command;
        }

        public System.Data.Common.DbParameter NewParameter()
        {
            return new SqlParameter();
        }
        public System.Data.Common.DbParameter NewParameter(string name, int value)
        {
            return new SqlParameter(name, value);
        }

        public System.Data.Common.DbParameter NewParameter(string name, object value)
        {
            if (value == null)
                value = DBNull.Value;
            return new SqlParameter(name, value);
        }
        public System.Data.Common.DbParameter NewParameter(string name, object value, DbDataType type)
        {
            SqlDbType sqlType = GenericToSql(type);
            var param = new SqlParameter(name, sqlType);
            if (value == null)
                value = DBNull.Value;
            param.Value = value;
            return param;
        }

        public System.Data.Common.DbParameter NewParameter(string name, DbDataType type)
        {
            SqlDbType sqlType = GenericToSql(type);
            return new SqlParameter(name, sqlType);
        }
        public System.Data.Common.DbParameter NewParameter(string name, DbDataType type, int size)
        {
             SqlDbType sqlType = GenericToSql(type);
             return new SqlParameter(name, sqlType, size);
        }
        public SqlDbType GenericToSql(DbDataType type)
        {
            switch (type)
            {
                case DbDataType.BIGINT:
                    return SqlDbType.BigInt;
                case DbDataType.BIT:
                case DbDataType.BOOL:
                case DbDataType.BOOLEAN:
                    return SqlDbType.Bit;
                case DbDataType.CHAR:
                    return SqlDbType.Char;
                case DbDataType.DATE:
                    return SqlDbType.Date;
                case DbDataType.DATETIME:
                    return SqlDbType.DateTime;
                case DbDataType.FLOAT:
                    return SqlDbType.Float;
                case DbDataType.INT:
                    return SqlDbType.Int;
                case DbDataType.MONEY:
                    return SqlDbType.Money;
                case DbDataType.SHORTDATETIME:
                    return SqlDbType.SmallDateTime;
                case DbDataType.SMALLINT:
                    return SqlDbType.SmallInt;
                case DbDataType.TEXT:
                    return SqlDbType.Text;
                case DbDataType.TIME:
                    return SqlDbType.Time;
                case DbDataType.TIMESTAMP:
                    return SqlDbType.Timestamp;
                case DbDataType.GUID:
                case DbDataType.UNIQUEIDENTIFIER:
                case DbDataType.UUID:
                    return SqlDbType.UniqueIdentifier;
                case DbDataType.VARBIN:
                case DbDataType.VARBINARY:
                case DbDataType.BINARYVAR:
                    return SqlDbType.VarBinary;
                case DbDataType.VARCHAR:
                    return SqlDbType.NVarChar;
                case DbDataType.IMAGE:
                    return SqlDbType.Image;
                case DbDataType.REAL:
                    return SqlDbType.Real;
                case DbDataType.DECIMAL:
                    return SqlDbType.Decimal;
                case DbDataType.TINYINT:
                    return SqlDbType.TinyInt;
                default:
                    throw new Exception("Unknown data type");
            }
        }


      
    }
}
