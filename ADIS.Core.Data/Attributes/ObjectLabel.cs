using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data
{
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    public class ObjectLabel : Attribute
    {
        public string label;
        public ObjectLabel(string label)
        {
            this.label = label;
        }
    }
}
