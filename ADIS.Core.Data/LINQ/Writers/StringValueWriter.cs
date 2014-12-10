using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data.LINQ.Writers.Interfaces;

namespace ADIS.Core.Data.LINQ.Writers
{
    internal class StringValueWriter : IValueWriter
    {
        public bool Handles(Type type)
        {

            return type == typeof(String);
        }

        
        public string WriteSqlFragment(object value)
        {
            return string.Format("'{0}'", value);
        }

        public string WriteUriFragment(object value)
        {
            return string.Format("'{0}'", value);
        }
    }
}
