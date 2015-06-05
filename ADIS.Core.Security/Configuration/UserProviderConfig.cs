using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Configuration;

namespace ADIS.Core.Security
{
    [ConfigurationEntity(ConfigurationEntityType.FileBound, "UserProviders")]
    public class UserProviderConfig
    {
        [ConfigurationProperty("MachineName")]
        public string MachineName
        {
            get;
            set;
        }
    }

}
