using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Security
{
    public class UserGroup
    {
        protected Guid id;
        protected IUserGroupBinding binding;
        protected bool enabled;

        public Guid ID { get { return id; } }
        public IUserGroupBinding Binding { get { return binding; } }
        public bool Enabled { get { return enabled; } }
    }
}
