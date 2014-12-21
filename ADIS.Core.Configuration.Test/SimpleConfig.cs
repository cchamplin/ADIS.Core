using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Configuration.Test
{
    [ConfigurationEntity(ConfigurationEntityType.DataBound,"Simple","ADIS_SIMPLE")]
    public class SimpleConfig
    {
        [ConfigurationProperty("PROP_A")]
        public int PropA
        {
            get;
            set;
        }
        [ConfigurationProperty("PROP_B")]
        public string PropB
        {
            get;
            set;
        }
    }
}
