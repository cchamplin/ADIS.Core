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

        IRoleBinding GetRoleProvider(string bindingMachineName);
        ISecurityGroupBinding GetSecurityGroupProvider(string bindingMachineName);
        IAuthenticationBinding GetAuthenticationProvider(string bindingMachineName);
        IUserGroupProvider GetUserGroupProvider(string bindingMachineName);

        List<Role> GetActiveRoles();
        List<Role> GetAllRoles();
        List<Role> GetInactiveRoles();

        List<SecurityGroup> GetActiveSecurityGroups();
        List<SecurityGroup> GetAllSecurityGroups();
        List<SecurityGroup> GetInactiveSecurityGroups();

        List<UserGroup> GetActiveUserGroups();
        List<UserGroup> GetAllUserGroups();
        List<UserGroup> GetInactiveUserGroups();

        User AuthenticateUser(string username, string password);
        User AuthenticateUser(object[] requestFields);
        
       
    }
}
