using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.ComponentServices.Database.Readers
{
    public class Int32Reader : ITypedReader
    {
        public object Read(DbRowReader reader, int ordinal, object[] data)
        {
            return reader.ReadInt32(ordinal, data);
        }
        public Type DataType
        {
            get { return typeof(Int32); }
        }
    }
}
