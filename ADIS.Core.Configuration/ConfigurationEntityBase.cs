using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Configuration
{
    public abstract class ConfigurationEntityBase<T> : IConfigurationEntity
    {
        protected List<T> items;
        public ConfigurationEntityBase()
        {
            items = new List<T>();
        }
        [ConfigurationProperty("ID")]
        public Guid ID
        {
            get;
            private set;
        }

        public List<T> Items
        {
            get
            {
                return items;
            }
        }
    }
}
