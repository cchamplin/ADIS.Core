using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Security;

namespace ADIS.Core.Data.Security
{
    public static class SecurableTypes
    {
        private static DataSecurableType doType;
        private static DataSecurableType proptype;
        public static ISecurableType DATA_OBJECT
        {
            get
            {
                if (doType == null)
                    doType = new DataSecurableType("DataObject","DATA_OBJECT");
                return doType;
            }
        }
        public static ISecurableType DATA_PROPERTY
        {
            get
            {
                if (proptype == null)
                    proptype = new DataSecurableType("DataProperty","DATA_PROPERTY");
                return proptype;
            }
        }
    }
}
