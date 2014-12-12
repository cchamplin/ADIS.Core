using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Configuration;

namespace ADIS.Core.Data.Configuration
{
    [ConfigurationEntity(ConfigurationEntityType.DataBound,"DBOConfig","ADIS_DBO")]
    public class DBOConfig : ConfigurationEntityBase<DBOConfig>
    {
        private DBOConfig()
        {
        }

        [ConfigurationProperty("NAME")]
        public string Name { get; set; }

        [ConfigurationProperty("NAMESPACE")]
        public string Namespace { get; set; }

        [ConfigurationProperty("ROUTE")]
        public string Route { get; set; }

        [ConfigurationProperty("ASSEMBLY")]
        public string Assembly { get; set; }

        public void RegisterDBO(string @namespace, string route, object dbo)
        {
            // Todo better type checking
            if (dbo is IDataBound)
            {
                var t = dbo.GetType();
                var assembly = t.Assembly.FullName;
                var name = t.Name;

                DBOConfig config = new DBOConfig();
                config.Name = name;
                config.Assembly = assembly;
                config.Route = route;
                config.Name = @namespace;
                items.Add(config);
            }
        }
    }
}
