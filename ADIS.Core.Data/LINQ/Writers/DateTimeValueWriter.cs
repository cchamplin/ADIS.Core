using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ADIS.Core.Data.LINQ.Writers.Interfaces;

namespace ADIS.Core.Data.LINQ.Writers
{
    internal class DateTimeValueWriter : IValueWriter
    {
        public bool Handles(Type type)
        {

            return type == typeof(DateTime);
        }

        public string WriteSqlFragment(object value)
        {
            var dateTimeValue = (DateTime)value;
            return string.Format("datetime'{0}'", XmlConvert.ToString(dateTimeValue, XmlDateTimeSerializationMode.Utc));

        }

        public string WriteUriFragment(object value)
        {
            var dateTimeValue = (DateTime)value;
            return string.Format("datetime'{0}'", XmlConvert.ToString(dateTimeValue, XmlDateTimeSerializationMode.Utc));

        }
    }
}
