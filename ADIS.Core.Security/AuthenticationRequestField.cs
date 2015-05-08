using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Security
{
    public class AuthenticationRequestField
    {

        protected string name;
        protected Type dataType;

        public AuthenticationRequestField(string name, Type dataType)
        {
            this.name = name;
            this.dataType = dataType;
        }

        public string Name { get { return name; } }
        public Type DataType { get { return dataType; } }
        public bool TryParse(byte[] rawInput, out object value)
        {
            value = null;
            return false;
        }
    }
}
