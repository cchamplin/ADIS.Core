using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.LINQ.Writers
{
    internal interface IResolvedValueWriter
    {
        bool Handles(Type type);
        string WriteSqlFragment(object value, INameResolver namedResolver);
        string WriteUriFragment(object value, INameResolver namedResolver);
    }
}
