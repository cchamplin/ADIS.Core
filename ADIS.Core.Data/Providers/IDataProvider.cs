using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data.SQL;

namespace ADIS.Core.Data.Providers
{
    interface IDataProvider
    {
        T FetchObject<T>(DataBoundObject<T> dbo, object keyValue, string op = "=") where T : DataBoundObject<T>;
       DBSelect<T> QueryObject<T>(DataBoundObject<T> dbo) where T : DataBoundObject<T>;
    }
}
