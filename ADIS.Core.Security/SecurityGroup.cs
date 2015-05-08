using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Security
{
    public class SecurityGroup
    {
        protected Guid id;
        protected ISecurityGroupBinding binding;
        protected SecurityGroup inherits;
        protected List<ISecurable> secured;
        protected bool enabled;

        public Guid ID { get { return id; } }
        public ISecurityGroupBinding Binding { get { return binding; } }
        public SecurityGroup Inherits { get { return inherits; } }
        public List<ISecurable> Secured { get { return secured; } }
        public bool Enabled { get { return enabled; } }

    }
}
