using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Security
{
    public class SecurityProviders : ISecurityProviders
    {

        public SecurityProviders()
        {
            securityGroupProviders = new Dictionary<string, ISecurityGroupProvider>();
            roleProviders = new Dictionary<string, IRoleProvider>();
            userGroupProviders = new Dictionary<string, IUserGroupProvider>();
            authenticationProviders = new Dictionary<string, IAuthenticationProvider>();
            userProviders = new Dictionary<string, IUserProvider>();
        }

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
            if (userProviders.ContainsKey(provider.Binding.MachineName))
            {
                throw new Exception("User provider has already been registered");
            }
            userProviders.Add(provider.Binding.MachineName, provider);
        }

        public IRoleProvider GetRoleProvider(string bindingMachineName)
        {
            if (!roleProviders.ContainsKey(bindingMachineName))
                throw new Exception("No such role provider has been registered");
            return roleProviders[bindingMachineName];
        }

        public ISecurityGroupProvider GetSecurityGroupProvider(string bindingMachineName)
        {
            if (!securityGroupProviders.ContainsKey(bindingMachineName))
                throw new Exception("No such security group has been registered");
            return securityGroupProviders[bindingMachineName];
        }

        public IAuthenticationProvider GetAuthenticationProvider(string bindingMachineName)
        {
            if (!authenticationProviders.ContainsKey(bindingMachineName))
                throw new Exception("No such authentication provider has been registered");
            return authenticationProviders[bindingMachineName];
        }
        public IUserGroupProvider GetUserGroupProvider(string bindingMachineName)
        {
            if (!userGroupProviders.ContainsKey(bindingMachineName))
                throw new Exception("No such user group provider has been registered");
            return userGroupProviders[bindingMachineName];
        }
        public IUserProvider GetUserProvider(string bindingMachineName)
        {
            if (!userProviders.ContainsKey(bindingMachineName))
                throw new Exception("No such user provider has been registered");
            return userProviders[bindingMachineName];
        }


       

        public List<Role> GetActiveRoles()
        {
            throw new NotImplementedException();
        }

        public List<Role> GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public List<Role> GetInactiveRoles()
        {
            throw new NotImplementedException();
        }

        public List<SecurityGroup> GetActiveSecurityGroups()
        {
            throw new NotImplementedException();
        }

        public List<SecurityGroup> GetAllSecurityGroups()
        {
            throw new NotImplementedException();
        }

        public List<SecurityGroup> GetInactiveSecurityGroups()
        {
            throw new NotImplementedException();
        }

        public List<UserGroup> GetActiveUserGroups()
        {
            throw new NotImplementedException();
        }

        public List<UserGroup> GetAllUserGroups()
        {
            throw new NotImplementedException();
        }

        public List<UserGroup> GetInactiveUserGroups()
        {
            throw new NotImplementedException();
        }

       
    }
}
