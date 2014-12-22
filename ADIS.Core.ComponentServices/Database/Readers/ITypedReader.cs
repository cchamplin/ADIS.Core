using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.ComponentServices.Database.Readers
{
    public interface ITypedReader
    {
        object Read(DbRowReader reader, int ordinal, object[] data);
        Type DataType { get; }
    }
}
