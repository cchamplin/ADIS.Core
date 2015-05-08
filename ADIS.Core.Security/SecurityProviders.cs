using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Security
{
    public class SecurityProviders : ISecurityProviders
    {
        protected Dictionary<string, ISecurityGroupProvider> securityGroupProviders;
        protected Dictionary<string, IRoleProvider> roleProviders;
        protected Dictionary<string, IAuthenticationProvider> authenticationProviders;
        protected Dictionary<string, IUserGroupProvider> userGroupProviders;
        protected Dictionary<string, IUserProvider> userProviders;
       

        public void RegisterSecurityGroupProvider(ISecurityGroupProvider provider)
        {
            if (provider.Binding == null)
            {
                throw new Exception("Security group provider binding cannot be null");
            }
            if (securityGroupProviders.ContainsKey(provider.Binding.MachineName)) {
                throw new Exception("Security group provider has already been registered");
            }
            securityGroupProviders.Add(provider.Binding.MachineName, provider);
        }

        public void RegisterUserGroupProvider(IUserGroupProvider provider)
        {
            if (provider.Binding == null)
            {
                throw new Exception("User group provider binding cannot be null");
            }
            if (userGroupProviders.ContainsKey(provider.Binding.MachineName))
            {
                throw new Exception("User group provider has already been registered");
            }
            userGroupProviders.Add(provider.Binding.MachineName, provider);
        }

        public void RegisterRoleProvider(IRoleProvider provider)
        {
            if (provider.Binding == null)
            {
                throw new Exception("Role provider binding cannot be null");
            }
            if (roleProviders.ContainsKey(provider.Binding.MachineName))
            {
                throw new Exception("Role provider has already been registered");
            }
            roleProviders.Add(provider.Binding.MachineName, provider);
        }

        public void RegisterAuthenticationProvider(IAuthenticationProvider provider)
        {
            if (provider.Binding == null)
            {
                throw new Exception("Authentication provider binding cannot be null");
            }
            if (authenticationProviders.ContainsKey(provider.Binding.MachineName))
            {
                throw new Exception("Authentication provider has already been registered");
            }
            authenticationProviders.Add(provider.Binding.MachineName, provider);
        }

        public void RegisterUserProvider(IUserProvider provider)
        {
            if (provider.Binding == null)
            {
                throw new Exception("User provider binding cannot be null");
            }
            if (provider.AuthenticationBinding == null)
            {
                throw new Exception("User provider authentication binding cannot be null");
            }
            if (userProviders.ContainsKey(provider.Binding.MachineName))
            {
                throw new Exception("User provider has already been registered");
            }
            userProviders.Add(provider.Binding.MachineName, provider);
        }

        public IRoleProvider GetRoleProvider(string bindingMachineName)
        {
            return roleProviders[bindingMachineName];
        }

        public ISecurityGroupProvider GetSecurityGroupProvider(string bindingMachineName)
        {
            return securityGroupProviders[bindingMachineName];
        }

        public IAuthenticationProvider GetAuthenticationProvider(string bindingMachineName)
        {
            return authenticationProviders[bindingMachineName];
        }
        public IUserGroupProvider GetUserGroupProvider(string bindingMachineName)
        {
            return userGroupProviders[bindingMachineName];
        }
        public IUserProvider GetUserProvider(string bindingMachineName)
        {
            return userProviders[bindingMachineName];
        }
    }
}
