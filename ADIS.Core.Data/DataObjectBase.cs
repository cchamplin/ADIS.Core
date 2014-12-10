using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data
{
    public abstract class DataObjectBase
    {


        [DataExempt]
        public abstract Dictionary<string, DataProperty> Properties { get; }
        

        [DataExempt]
        public abstract List<DataProperty> PropertyList { get; }
        
    }
}
