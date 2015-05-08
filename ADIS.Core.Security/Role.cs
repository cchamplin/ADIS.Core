using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Security
{
    public class Role
    {
        protected Guid id;
        protected IRoleBinding binding;
        protected List<SecurityGroup> securityGroups;
        protected bool enabled;

        public Guid ID { get { return id; } }
        public IRoleBinding Binding { get { return binding; } }
        public List<SecurityGroup> SecurityGroups { get { return securityGroups; } }
        public bool Enabled { get { return enabled; } }
    }
}
