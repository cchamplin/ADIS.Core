﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.ComponentServices.Database.Readers
{
    public class ByteArrayReader : ITypedReader
    {
        public object Read(DbRowReader reader, int ordinal, object[] data)
        {
            return reader.ReadByteArray(ordinal, data);
        }
        public Type DataType
        {
            get { return typeof(Byte[]); }
        }
    }
}
