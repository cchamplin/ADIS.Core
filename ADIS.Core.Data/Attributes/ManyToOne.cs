using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class ManyToOne : Attribute
    {
        public string KeyColumn;
        public ManyToOne(string keyColumn)
        {
            this.KeyColumn = keyColumn;
        }
    }
}
