using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data
{
    public class DataBoundProperty : DataProperty
    {
        private string columnName;
        public DataBoundProperty(Type t, PropertyInfo pi)
            : base(t,pi)
        {
             foreach (Attribute attr in pi.GetCustomAttributes(false)) {
               if (attr is PropertyColumn) {
                   this.columnName = ((PropertyColumn)attr).columnName;
               }
            }
        }
        public string ColumnName
        {
            get
            {
                return columnName;
            }
        }
    }
}
