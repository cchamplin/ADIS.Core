using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.LINQ.Writers.Interfaces
{
    internal interface IValueWriter
    {
        bool Handles(Type type);
        string WriteSqlFragment(object value);
        string WriteUriFragment(object value);
    }

}
