using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Configuration
{
    [ConfigurationEntity(ConfigurationEntityType.FileBound,"ConnectionString")]
    public class ConnectionStringConfigRecord
    {
        protected string name;
        protected string connectionString;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }
    }
}
