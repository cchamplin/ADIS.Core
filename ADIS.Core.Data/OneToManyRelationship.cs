using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data
{
    public class OneToManyRelationship : DataRelationship
    {
        public OneToManyRelationship(string keyColumn)
            : base(keyColumn)
        {
        }
    }
}
