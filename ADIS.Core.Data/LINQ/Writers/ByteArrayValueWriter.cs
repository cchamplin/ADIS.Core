using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data.LINQ.Writers.Interfaces;

namespace ADIS.Core.Data.LINQ.Writers
{
    internal class ByteArrayValueWriter : IValueWriter
    {
        
        public bool Handles(Type type)
        {

            return type == typeof(byte[]);
        }
        public string WriteSqlFragment(object value)
        {
            var base64 = Convert.ToBase64String((byte[])value);
            return string.Format("X'{0}'", base64);
        }

        public string WriteUriFragment(object value)
        {
            var base64 = Convert.ToBase64String((byte[])value);
            return string.Format("X'{0}'", base64);
        }
    }
}
