using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Security
{
    public class LDAPUserBinding : IUserBinding
    {
        public string MachineName
        {
            get { return "LDAP_USER_BINDING"; }
        }

        public string Name
        {
            get { return "LDAP Users"; }
        }


        public string EndPoint
        {
            get { return "ldap"; }
        }
    }
}
