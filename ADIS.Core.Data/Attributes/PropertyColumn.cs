using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class PropertyColumn : Attribute
    {
        public string columnName;
        public PropertyColumn(string name)
        {
            this.columnName = name;
        }
    }
}
