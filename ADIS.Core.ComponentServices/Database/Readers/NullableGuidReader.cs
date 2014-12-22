using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.ComponentServices.Database.Readers
{
    public class NullableGuidReader : ITypedReader
    {
        public object Read(DbRowReader reader, int ordinal, object[] data)
        {
            return reader.ReadNullableGuid(ordinal, data);
        }
        public Type DataType
        {
            get { return typeof(Guid?); }
        }
    }
}
