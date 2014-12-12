using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class DataMember : Attribute
    {
        public string columnName;
        public bool primaryKey;
        public DataMember(string columnName, bool primaryKey = false)
        {
            this.columnName = columnName;
            this.primaryKey = primaryKey;
        }
    }
}
