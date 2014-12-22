using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.ComponentServices.Database.Readers
{
    public class NullableInt32Reader : ITypedReader
    {
        public object Read(DbRowReader reader, int ordinal, object[] data)
        {
            return reader.ReadNullableInt32(ordinal, data);
        }
        public Type DataType
        {
            get { return typeof(Int32?); }
        }
    }
}
