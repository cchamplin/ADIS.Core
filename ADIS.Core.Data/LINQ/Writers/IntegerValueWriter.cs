using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data.LINQ.Writers.Interfaces;

namespace ADIS.Core.Data.LINQ.Writers
{
    internal abstract class IntegerValueWriter : IValueWriter
    {
       
        public string WriteSqlFragment(object value)
        {
            return value.ToString();
        }

        public string WriteUriFragment(object value)
        {
            return value.ToString();
        }

        public abstract bool Handles(Type type);
    }
}
