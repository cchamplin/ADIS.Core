using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data
{
    public abstract class DataRelationship
    {
        public DataRelationship(string keyColumn)
        {
            this.keyColumn = keyColumn;
        }
        protected string keyColumn;
        public string KeyColumn
        {
            get { return keyColumn; }
        }
    }
}
