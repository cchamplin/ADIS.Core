using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Security
{
    public class SqlAuthenticationBinding : IAuthenticationBinding
    {
        public string MachineName
        {
            get { return "SQL_AUTHENTICATION_BINDING"; }
        }

        public string Name
        {
            get { return "SQL Authentication"; }
        }
    }
}
