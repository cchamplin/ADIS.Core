using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ADIS.Core.Data.LINQ.Writers.Interfaces;

namespace ADIS.Core.Data.LINQ.Writers
{
    internal class TimeSpanValueWriter : IValueWriter
    {
        public bool Handles(Type type)
        {

            return type == typeof(TimeSpan);
        }

          public string WriteSqlFragment(object value)
        {
            return string.Format("time'{0}'", XmlConvert.ToString((TimeSpan)value));
        }

        public string WriteUriFragment(object value)
        {
            return string.Format("time'{0}'", XmlConvert.ToString((TimeSpan)value));
        }
    }
}
