using ADIS.Core.ComponentServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Configuration;
using FastSerialize;

namespace ADIS.Core.Security
{
    public class AuthenticationServiceHandler : IRequestHandler
    {
        protected ISecurityProviders securityProviders = null;
        protected List<IUserProvider> userProviders;
        protected List<IAuthenticationProvider> authenticationProviders;
        protected ISerializer serializer;
        public AuthenticationServiceHandler()
        {
            ISecurityProviders securityProviders = ComponentServices.ComponentServices.Fetch("Security").Resolve<ISecurityProviders>();
            this.securityProviders = securityProviders;

            
            var cm = ConfigurationManager.Current;

            var providers = cm.BindAll<UserProviderConfig>();
            userProviders = new List<IUserProvider>();
            foreach (var provider in providers)
            {
                userProviders.Add(this.securityProviders.GetUserProvider(provider.MachineName));
            }

            var authProviders = cm.BindAll<AuthenticationProviderConfig>();
            authenticationProviders = new List<IAuthenticationProvider>();
            foreach (var provider in authProviders)
            {
                authenticationProviders.Add(this.securityProviders.GetAuthenticationProvider(provider.MachineName));
            }

             var textServices = ADIS.Core.ComponentServices.ComponentServices.Fetch("Text");
             serializer = textServices.Resolve<ISerializer>();
           
        }
        public object Handle(IRequest request, IRoute route)
        {

            if (request.Method == "POST")
            {
                var compontents = route.GetComponents(request.Url);
                IUserProvider userProvider = null;
                IAuthenticationProvider authenticationProvider = null;
                if (compontents != null)
                {
                    if (compontents.ContainsKey("userbinding"))
                    {
                        if (compontents["userbinding"] == "default" || compontents["userbinding"] == "_" || compontents["userbinding"] == "")
                        {
                            // Load Default
                        }
                        foreach (var provider in userProviders)
                        {
                            if (compontents["userbinding"] == provider.Binding.EndPoint)
                            {
                                userProvider = provider;
                                break;
                            }
                        }
                        if (userProvider == null)
                        {
                            throw new Exception("Invalid user provider specified");
                        }
                    }
                    else
                    {
                        // Load Default
                    }
                    if (compontents.ContainsKey("authenticationbinding"))
                    {
                        if (compontents["authenticationbinding"] == "default" || compontents["authenticationbinding"] == "_" || compontents["authenticationbinding"] == "")
                        {
                            // Load Default
                        }
                        foreach (var provider in authenticationProviders)
                        {
                            if (compontents["authenticationbinding"] == provider.Binding.EndPoint)
                            {
                                authenticationProvider = provider;
                                break;
                            }
                        }
                        if (authenticationProvider == null)
                        {
                            throw new Exception("Invalid authentication provider specified");
                        }
                    }
                    else
                    {
                        // Load Default
                    }
                    
                }
                else
                {
                    // Get default bindings
                }
                var data = request.Data;
                if (string.IsNullOrEmpty(data))
                    throw new Exception("Invalid authentication data received");
                object authPacket;
                try
                {
                    authPacket = serializer.Deserialize(authenticationProvider.AuthenticationRequestType, data);

                }
                catch (Exception ex)
                {
                    throw new Exception("Unable to validate authentication data");
                }
                if (authPacket == null)
                {
                    throw new Exception("Unable to validate authentication data");
                }

                var user = authenticationProvider.GetUser(authPacket, userProvider);

                if (user == null)
                {
                    throw new Exception("No such user");
                }
                return authenticationProvider.AuthenticateUser(user, authPacket);
                
            }
            throw new Exception("Invalid request type");
        }
    }
}
