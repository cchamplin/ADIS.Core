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
        protected Dictionary<Type, dynamic> configurations;
        private ConfigurationManager()
        {
            configurations = new Dictionary<Type, dynamic>();
        }
        public static ConfigurationManager Current
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
        private static ConfigurationManager LoadConfiguration()
        {
            return new ConfigurationManager();
        }

        public dynamic Bind<T>()
        {
            return configurations[typeof(T)];
        }
        public IList<dynamic> BindAll<T>()
        {
            var result = new ConfigurationItemList();
            return (IList<dynamic>)result;
        }
	

        public void AddConfiguration(string name, dynamic configuration)
        {
        }
    }
}
