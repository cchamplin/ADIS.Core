using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Security
{
    public interface ISecurityProviders
    {
        void RegisterSecurityGroupProvider(ISecurityGroupProvider provider);
        void RegisterRoleProvider(IRoleProvider provider);
        void RegisterAuthenticationProvider(IAuthenticationProvider provider);
        void RegisterUserGroupProvider(IUserGroupProvider provider);
        void RegisterUserProvider(IUserProvider provider);

        IRoleProvider GetRoleProvider(string bindingMachineName);
        ISecurityGroupProvider GetSecurityGroupProvider(string bindingMachineName);
        IAuthenticationProvider GetAuthenticationProvider(string bindingMachineName);
        IUserGroupProvider GetUserGroupProvider(string bindingMachineName);
        IUserProvider GetUserProvider(string bindingMachineName);

        List<Role> GetActiveRoles();
        List<Role> GetAllRoles();
        List<Role> GetInactiveRoles();

        List<SecurityGroup> GetActiveSecurityGroups();
        List<SecurityGroup> GetAllSecurityGroups();
        List<SecurityGroup> GetInactiveSecurityGroups();

        List<UserGroup> GetActiveUserGroups();
        List<UserGroup> GetAllUserGroups();
        List<UserGroup> GetInactiveUserGroups();
        
       
    }
}
