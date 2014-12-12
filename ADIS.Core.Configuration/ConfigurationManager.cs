using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Configuration
{
    public class ConfigurationManager
    {
        private static ConfigurationManager instance;
        protected Dictionary<string, ConfigurationEntity> configurations;
        private ConfigurationManager()
        {
            configurations = new Dictionary<string, ConfigurationEntity>();
        }
        public ConfigurationManager Current
        {
            get
            {
                if (instance == null)
                {
                    instance = LoadConfiguration();
                }
                return instance;
            }
        }
        private ConfigurationManager LoadConfiguration()
        {
            return null;
        }

        public ConfigurationEntity this[string item]
        {
            get
            {
                return configurations[item];
            }
        }

        public void AddConfiguration(string name, ConfigurationEntity configuration)
        {
        }
    }
}
