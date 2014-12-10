using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ADIS.Core.Data.LINQ.Writers.Interfaces;

namespace ADIS.Core.Data.LINQ.Writers
{
    internal class DateTimeOffsetValueWriter : IValueWriter
    {
        public bool Handles(Type type)
        {

            return type == typeof(DateTimeOffset);
        }
        public string WriteSqlFragment(object value)
        {
            return string.Format("datetimeoffset'{0}'", XmlConvert.ToString((DateTimeOffset)value));
        }

        public string WriteUriFragment(object value)
        {
            return string.Format("datetimeoffset'{0}'", XmlConvert.ToString((DateTimeOffset)value));
        }
    }
}
