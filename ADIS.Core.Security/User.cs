using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Security
{
    public class User
    {
        protected Guid ID;

        protected string loginname;
        protected string passSalt;
        protected string passHash;
        protected List<Role> roles;
        protected List<UserGroup> userGroups;
        protected List<SecurityGroup> securityGroups;
        protected bool isAdministrator;
        protected IUserType userType;
        protected DateTime changeDate;
        protected DateTime addedDate;
        protected Guid changeID;
        protected Guid addID;

        protected DateTime lastLogin;
        protected DateTime firstLogin;
        protected int numLogins;

        protected bool expires;
        protected DateTime expiresDate;

        protected List<IAccessAvailability> accessAvailability;

        protected IAuthenticationBinding bindingType;

        protected Dictionary<string, IDataSetting> settings;


    }
}
