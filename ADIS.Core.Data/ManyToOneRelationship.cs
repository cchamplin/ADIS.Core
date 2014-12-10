using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data
{
    public class ManyToOneRelationship :DataRelationship
    {
        public ManyToOneRelationship(string keyColumn)
            : base(keyColumn)
        {
        }
    }
}
