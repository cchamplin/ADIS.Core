using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Configuration
{
    [ConfigurationEntity(ConfigurationEntityType.FileBound,"ServiceContainers")]
    public class ServiceContainerConfigRecord
    {
        protected string name;
        protected List<ServiceConfigRecord> services;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public List<ServiceConfigRecord> Services
        {
            get { return services; }
            set { services = value; }
        }
    }
}
