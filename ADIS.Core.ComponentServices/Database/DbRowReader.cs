using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.ComponentServices.Database
{
    public class DbRowReader : RowReader
    {
        private readonly DbDataReader reader;
        public DbRowReader(DbDataReader reader)
        {
            this.reader = reader;
            this.Init();
        }

        protected override int FieldCount
        {
            get { return this.reader.FieldCount; }
        }

        protected override Type GetFieldType(int ordinal)
        {
            return this.reader.GetFieldType(ordinal);
        }

        protected override bool IsDBNull(int ordinal, object[] data)
        {
            return data[ordinal] is DBNull;//this.reader.IsDBNull(ordinal);
        }

        protected override T GetValue<T>(int ordinal, object[] data)
        {
            return (T)((object)data[ordinal].ToString());

            // return (T)this.executor.Convert(this.reader.GetValue(ordinal), typeof(T));
        }

        protected override Byte GetByte(int ordinal, object[] data)
        {
            return (Byte)data[ordinal];
        }
        protected override Boolean GetBoolean(int ordinal, object[] data)
        {
            if (data[ordinal] is bool)
                return (Boolean)data[ordinal];
            else if (data[ordinal] is Boolean)
                return (Boolean)data[ordinal];
            else
                return ((int)data[ordinal] > 0) ? true : false;

        }

        protected override Char GetChar(int ordinal, object[] data)
        {
            return (Char)data[ordinal];
        }

        protected override DateTime GetDateTime(int ordinal, object[] data)
        {
            return (DateTime)data[ordinal];
        }

        protected override Decimal GetDecimal(int ordinal, object[] data)
        {
            return (Decimal)data[ordinal];
        }

        protected override Double GetDouble(int ordinal, object[] data)
        {
            return (Double)data[ordinal];
        }

        protected override Single GetSingle(int ordinal, object[] data)
        {
            return (Single)data[ordinal];
        }

        protected override Guid GetGuid(int ordinal, object[] data)
        {
            return (Guid)data[ordinal];
        }

        protected override Byte[] GetByteArray(int ordinal, object[] data)
        {
            return (Byte[])data[ordinal];
        }

        protected override Char[] GetCharArray(int ordinal, object[] data)
        {
            return (Char[])data[ordinal];
        }

        protected override Int16 GetInt16(int ordinal, object[] data)
        {
            return (Int16)data[ordinal];
        }

        protected override Int32 GetInt32(int ordinal, object[] data)
        {
            return (Int32)data[ordinal];
        }

        protected override Int64 GetInt64(int ordinal, object[] data)
        {
            return (Int64)data[ordinal];
        }

        protected override String GetString(int ordinal, object[] data)
        {
            return (String)data[ordinal];
        }
    }
}
