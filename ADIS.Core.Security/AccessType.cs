using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Security
{
    public enum AccessType
    {
        NONE = 0,
        READ = 2,
        UPDATE = 4,
        CREATE = 8,
        DELETE = 16
    }
}
