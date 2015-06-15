using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Security
{
    public class SystemUserType : IUserType
    {
        public string MachineName
        {
            get { return "SYSTEM"; }
        }

        public string Name
        {
            get { return "SYSTEM"; }
        }
    }
}
