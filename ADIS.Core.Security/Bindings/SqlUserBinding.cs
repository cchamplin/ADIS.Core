using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Security
{
    public class SqlUserBinding : IUserBinding
    {
        public string MachineName
        {
            get { return "SQL_USER_BINDING"; }
        }

        public string Name
        {
            get { return "SQL Users"; }
        }


        public string EndPoint
        {
            get { return "sql"; }
        }
    }
}
