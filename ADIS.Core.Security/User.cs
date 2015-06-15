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
        protected string email;

        protected List<Role> roles;
        protected List<UserGroup> userGroups;
        protected List<SecurityGroup> securityGroups;
        
        protected bool isAdministrator;
        
        protected IUserType userType;
        
        protected DateTime changeDate;
        protected DateTime addedDate;
        protected Guid changeID;
        protected Guid addID;

        protected DateTime? lastLogin;
        protected DateTime? firstLogin;
        protected int numLogins;

        protected bool expires;
        protected DateTime? expiresDate;

        protected List<IAccessAvailability> accessAvailability;

        protected IUserBinding userBindingType;
        protected IAuthenticationBinding authenticationBindingType;

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
        public DateTime? LastLogin
        {
            get { return lastLogin; }
            set { lastLogin = value; }
        }

        public DateTime? FirstLogin
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

        public DateTime? ExpiresDate
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



        public bool ComparePassword(string password)
        {
            return Crypto.ValidatePassword(password, this.passSalt, this.passHash);
        }

        public bool IsAdministrator
        {
            get { return isAdministrator; }
            set { isAdministrator = value; }
        }

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
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
            get { return addID; }
            set { addID = value; }
        }

        internal IUserBinding UserBinding
        {
            get
            {
                return userBindingType;
            }
            set
            {
                this.userBindingType = value;
            }
        }
        internal IAuthenticationBinding AuthenticationBinding
        {
            get
            {
                return authenticationBindingType;
            }
            set
            {
                this.authenticationBindingType = value;
            }
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
