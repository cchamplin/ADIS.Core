using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Security
{
    public class LDAPAuthenticationBinding : IAuthenticationBinding
    {
        public string MachineName
        {
            get { return "LDAP_AUTHENTICATION_BINDING"; }
        }

        public string Name
        {
            get { return "LDAP Authentication"; }
        }
    }
}
