using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Configuration;

namespace ADIS.Core.Security
{
    [ConfigurationEntity(ConfigurationEntityType.FileBound, "AuthenticationProviders")]
    public class AuthenticationProviderConfig
    {
        [ConfigurationProperty("MachineName")]
        public string MachineName
        {
            get;
            set;
        }
        [ConfigurationProperty("Default")]
        public bool Default
        {
            get;
            set;
        }
    }

}
