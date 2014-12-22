﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.ComponentServices.Database.Readers
{
    public class ValueReader<T> : ITypedReader
    {
        public object Read(DbRowReader reader, int ordinal, object[] data)
        {
            return reader.ReadValue<T>(ordinal, data);
        }
        public Type DataType
        {
            get { return typeof(T); }
        }
    }
}
