using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data.LINQ.Writers.Interfaces;

namespace ADIS.Core.Data.LINQ.Writers
{
    internal abstract class RationalValueWriter : IValueWriter
    {
        public abstract bool Handles(Type type);
        public string WriteSqlFragment(object value)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}", value);
        }

        public string WriteUriFragment(object value)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}", value);
        }
    }
}
