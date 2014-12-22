using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.ComponentServices.Database.Readers
{
    public class NullableDateTimeReader : ITypedReader
    {
        public object Read(DbRowReader reader, int ordinal, object[] data)
        {
            return reader.ReadNullableDateTime(ordinal, data);
        }
        public Type DataType
        {
            get { return typeof(DateTime?); }
        }
    }
}
