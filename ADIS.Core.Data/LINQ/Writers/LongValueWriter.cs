﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.LINQ.Writers
{
    internal class LongValueWriter : IntegerValueWriter
    {
        public override bool Handles(Type type)
        {

            return type == typeof(long);
        }
    }
}
