using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class DataMemberType : Attribute
    {
        protected SqlDbType type;
        public DataMemberType(SqlDbType type)
        {
            this.type = type;
        }
        public SqlDbType Type
        {
            get
            {
                return type;
            }
        }

    }
}
