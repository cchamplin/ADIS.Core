using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Security
{
    public class SqlAuthenticationProvider : IAuthenticationProvider
    {
        protected IAuthenticationBinding binding;
        public SqlAuthenticationProvider(IAuthenticationBinding binding)
        {
            this.binding = binding;
        }
        public IAuthenticationBinding Binding
        {
            get { return binding; }
        }

        public object AuthenticateUser(User user, object authenticationRequest)
        {

            if (authenticationRequest is UsernamePasswordRequest)
            {
                var authRequest = authenticationRequest as UsernamePasswordRequest;
                if (authenticationRequest == null)
                    throw new Exception("Invalid authentication request");
                if (user.ComparePassword(authRequest.Password))
                {
                    return new JsonWebToken("ADIS", user.UserID.ToString(), DateTime.Now.AddMinutes(15));
                }
            }
            throw new Exception("Invalid authentication request");
        }

        public User GetUser(object authenticationRequest, IUserProvider userProvider)
        {
            if (authenticationRequest is UsernamePasswordRequest)
            {
                var authRequest = authenticationRequest as UsernamePasswordRequest;
                if (authenticationRequest == null)
                    throw new Exception("Invalid authentication request");
                return userProvider.GetByUsername(((UsernamePasswordRequest)authenticationRequest).Username);
            }
            throw new Exception("Invalid authentication request");
        }

        public Type AuthenticationRequestType
        {
            get { return typeof(UsernamePasswordRequest); }
        }
    }
}
