using ADIS.Core.ComponentServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Configuration;

namespace ADIS.Core.Security
{
    public class AuthenticationServiceHandler : IRequestHandler
    {
        ISecurityProviders securityProviders = null;
        List<IUserProvider> userProviders;
        public AuthenticationServiceHandler()
        {
            ISecurityProviders securityProviders = ComponentServices.ComponentServices.Fetch("Security").Resolve<ISecurityProviders>();
            this.securityProviders = securityProviders;

            
            var cm = ConfigurationManager.Current;

            var providers = cm.BindAll<UserProviderConfig>();
            foreach (var provider in providers)
            {
                userProviders.Add(this.securityProviders.GetUserProvider(provider.MachineName));
            }
           
        }
        public object Handle(IRequest request, IRoute route)
        {



            if (request.Method == "POST")
            {
                var compontents = route.GetComponents(request.Url);
                IUserProvider userProvider = null;
                IAuthenticationBinding authenticationBinding = null;
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
                        foreach (var provider in userProviders)
                        {
                            if (compontents["authenticationbinding"] == provider.Binding.EndPoint)
                            {
                                authenticationBinding = provider;
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
                    
                }
                else
                {
                    // Get default bindings
                }
            }
            throw new Exception("Invalid request type");
        }
    }
}
