using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Security
{
    public class Role
    {
        protected Guid ID;
        protected string machineName;
        protected string name;
        protected IRoleBinding binding;
    }
}
