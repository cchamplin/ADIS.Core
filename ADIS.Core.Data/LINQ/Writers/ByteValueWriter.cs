using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data.LINQ.Writers.Interfaces;

namespace ADIS.Core.Data.LINQ.Writers
{
    internal class ByteValueWriter : IValueWriter
    {
        public bool Handles(Type type)
        {

            return type == typeof(byte);
        }
        public string WriteSqlFragment(object value)
        {
            var byteValue = (byte)value;

            return byteValue.ToString("X");
        }

        public string WriteUriFragment(object value)
        {
            var byteValue = (byte)value;

            return byteValue.ToString("X");
        }
    }
}
