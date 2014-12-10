using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data.LINQ.Writers.Interfaces;

namespace ADIS.Core.Data.LINQ.Writers
{
    internal class StreamValueWriter : IValueWriter
    {
        public bool Handles(Type type)
        {

            return type == typeof(Stream);
        }

 
        public string WriteSqlFragment(object value)
        {
            var stream = (Stream)value;
            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            var base64 = Convert.ToBase64String(buffer);

            return string.Format("X'{0}'", base64);
        }

        public string WriteUriFragment(object value)
        {
            var stream = (Stream)value;
            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            var base64 = Convert.ToBase64String(buffer);

            return string.Format("X'{0}'", base64);
        }
    }
}
