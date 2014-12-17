using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADIS.Core.Security;

namespace ADIS.Core.Data.Security
{
    internal class DataSecurableType : ISecurableType
    {
        protected string machineName;
        protected string name;
        internal DataSecurableType(string name, string machineName)
        {
            this.machineName = machineName;
            this.name = name;

        }
        public string MachineName
        {
            get
            {
                return machineName;
            }
            protected set
            {
                machineName = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            protected set
            {
                name = value;
            }
        }

    }
}
