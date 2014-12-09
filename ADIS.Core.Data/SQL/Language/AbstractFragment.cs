using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.SQL
{
    public abstract class AbstractFragment
    {
        public abstract string ToSQL(FragmentContext context);
    }
}
