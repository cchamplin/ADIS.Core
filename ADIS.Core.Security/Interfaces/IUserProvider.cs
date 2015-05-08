using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Security
{
    public interface IUserProvider
    {
        IAuthenticationBinding AuthenticationBinding { get; }
        IUserBinding Binding { get; }
        User GetByEmail(string emailAddress);
        User GetByUsername(string username);
        User GetByGuid(Guid identifier);
        User GetByToken(object token);

        void Register(User user);

        
    }
}
