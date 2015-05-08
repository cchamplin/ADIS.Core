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

        protected IUserBinding bindingType;

        protected Dictionary<string, IDataSetting> settings;

        public Guid UserID
        {
            get { return ID; }
            set { ID = value; }
            
        }
        public DateTime AddedDate {
            get { return addedDate; }
            set { addedDate = value; }
        }
        public DateTime LastLogin
        {
            get { return lastLogin; }
            set { lastLogin = value; }
        }

        public DateTime FirstLogin
        {
            get { return firstLogin; }
            set { firstLogin = value; }
        }

        public int NumLogins
        {
            get { return numLogins; }
            set { numLogins = value; }
        }

        public bool Expires
        {
            get { return expires; }
            set { expires = value; }
        }

        public DateTime ExpiresDate
        {
            get { return expiresDate; }
            set { expiresDate = value; }
        }

        public string LoginName
        {
            get { return loginname; }
            set { loginname = value; }
        }
        public string PassSalt
        {
            protected get { return passSalt; }
            set { passSalt = value; }
        }
        public string PassHash
        {
            protected get { return passHash; }
            set { passHash = value; }
        }

        public bool IsAdministrator
        {
            get { return isAdministrator; }
            set { isAdministrator = value; }
        }

        public IUserType UserType
        {
            get { return userType; }
            set { userType = value; }
        }

        public DateTime ChangedDate
        {
            get { return changeDate; }
            set { changeDate = value; }
        }

        public Guid ChangeID
        {
            get { return changeID; }
            set { changeID = value; }
        }


        public Guid AddedID
        {
            get { return AddedID; }
            set { AddedID = value; }
        }


        public List<Role> Roles
        {
            get { return roles; }
            set { roles = value; }
        }
        public List<UserGroup> UserGroups {
            get { return userGroups; }
            set { userGroups = value; }
        }
        public List<SecurityGroup> SecurityGroups {
            get { return securityGroups; }
            set { securityGroups = value; }
        }

    }
}
