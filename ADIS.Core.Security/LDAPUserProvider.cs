using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.ComponentServices;
using ADIS.Core.ComponentServices.Database;
using ADIS.Core.Configuration;

namespace ADIS.Core.Security
{
    public class LDAPUserProvider : IUserProvider
    {
        protected IUserBinding binding;
        protected string connectionString;
        protected IDatabaseProvider dbProvider;
        protected ISecurityProviders securityProviders;
        public LDAPUserProvider(IUserBinding binding)
        {
            ISecurityProviders securityProviders = ComponentServices.ComponentServices.Fetch("Security").Resolve<ISecurityProviders>();
            this.securityProviders = securityProviders;

            IDatabaseProvider dbProvider = ComponentServices.ComponentServices.Fetch("Data").Resolve<IDatabaseProvider>();
            this.dbProvider = dbProvider;

            var cm = ConfigurationManager.Current;

            var strings = cm.BindAll<ConnectionStringConfigRecord>();
            this.connectionString = strings.Where(x => x.Name == "Default").First().ConnectionString;
        }
        public IUserBinding Binding
        {
            get { return this.binding; }
        }

        protected User GetUser(DbDataReader dr, DbRowReader reader)
        {
            object[] data = new object[dr.FieldCount];
            dr.GetValues(data);
            var user = new User()
            {
                AddedDate = reader.ReadDateTime(dr.GetOrdinal("ADDED_DATE"),data),
                AddedID = reader.ReadGuid(dr.GetOrdinal("ADDED_GU"),data),
                ChangedDate = reader.ReadDateTime(dr.GetOrdinal("CHANGE_DATE"),data),
                ChangeID = reader.ReadGuid(dr.GetOrdinal("CHANGE_GU"), data),
                Expires = reader.ReadBoolean(dr.GetOrdinal("EXPIRES"), data),
                ExpiresDate = reader.ReadDateTime(dr.GetOrdinal("EXPIRES_DATE"),data),
                FirstLogin = reader.ReadDateTime(dr.GetOrdinal("FIRST_LOGIN"),data),
                IsAdministrator = reader.ReadBoolean(dr.GetOrdinal("IS_ADMINISTRATOR"),data),
                LastLogin = reader.ReadDateTime(dr.GetOrdinal("LAST_LOGIN"), data),
                LoginName = reader.ReadString(dr.GetOrdinal("LOGIN_NAME"),data),
                NumLogins = reader.ReadInt32(dr.GetOrdinal("NUM_LOGINS"),data),
                PassHash = reader.ReadString(dr.GetOrdinal("PASSWORD"),data),
                PassSalt = reader.ReadString(dr.GetOrdinal("PASSWORD_SALT"),data),
                Roles = new List<Role>(),
                SecurityGroups = new List<SecurityGroup>(),
                UserGroups = new List<UserGroup>(),
                UserID = reader.ReadGuid(dr.GetOrdinal("USER_GU"), data),
                UserType = null,
                AuthenticationBinding = securityProviders.GetAuthenticationProvider(reader.ReadString(dr.GetOrdinal("AUTHENTICATION_BINDING_TYPE"),data))
            };
            return user;
        }

        public User GetByEmail(string emailAddress)
        {
            DbConnection dbConnection = null;
            if (dbProvider == null || dbConnection == null)
            {
                dbConnection = dbProvider.NewConnection(connectionString);
                dbConnection.Open();
            }
            if (dbProvider != null)
            {
                if (dbConnection.State == System.Data.ConnectionState.Closed)
                {
                    dbConnection.Open();
                }
                if (dbConnection.State == System.Data.ConnectionState.Broken)
                {
                    dbConnection.Close();
                    dbConnection.Open();
                }
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT * FROM [adis].[ADIS_USER] WHERE email=@email AND USER_BINDING_TYPE=@bindingMachineName;");
                    DbCommand cmd = dbConnection.CreateCommand();
                    cmd.Connection = dbConnection;
                    System.Diagnostics.Debug.WriteLine("Running SQL : " + sb.ToString());
                    cmd.CommandText = sb.ToString();
                    cmd.Parameters.Add(dbProvider.NewParameter("@bindingMachineName", binding.MachineName));
                    cmd.Parameters.Add(dbProvider.NewParameter("@email", emailAddress));
                    var reader = cmd.ExecuteReader();
                    List<User> users = new List<User>();
                    var rowReader = new DbRowReader(reader);
                    reader.Read();
                    var user = GetUser(reader,rowReader);
                    
                    return user;


                }
                return null;
            }
            throw new Exception("Unable to query user data");
        }

        public User GetByUsername(string username)
        {
            DbConnection dbConnection = null;
            if (dbProvider == null || dbConnection == null)
            {
                dbConnection = dbProvider.NewConnection(connectionString);
                dbConnection.Open();
            }
            if (dbProvider != null)
            {
                if (dbConnection.State == System.Data.ConnectionState.Closed)
                {
                    dbConnection.Open();
                }
                if (dbConnection.State == System.Data.ConnectionState.Broken)
                {
                    dbConnection.Close();
                    dbConnection.Open();
                }
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT * FROM [adis].[ADIS_USER] WHERE LOGIN_NAME=@username AND USER_BINDING_TYPE=@bindingMachineName;");
                    DbCommand cmd = dbConnection.CreateCommand();
                    cmd.Connection = dbConnection;
                    System.Diagnostics.Debug.WriteLine("Running SQL : " + sb.ToString());
                    cmd.CommandText = sb.ToString();
                    cmd.Parameters.Add(dbProvider.NewParameter("@bindingMachineName", binding.MachineName));
                    cmd.Parameters.Add(dbProvider.NewParameter("@username", username));
                    var reader = cmd.ExecuteReader();
                    List<User> users = new List<User>();
                    var rowReader = new DbRowReader(reader);
                    reader.Read();
                    var user = GetUser(reader, rowReader);

                    return user;


                }
                return null;
            }
            throw new Exception("Unable to query user data");
        }

        public User GetByGuid(Guid identifier)
        {
            DbConnection dbConnection = null;
            if (dbProvider == null || dbConnection == null)
            {
                dbConnection = dbProvider.NewConnection(connectionString);
                dbConnection.Open();
            }
            if (dbProvider != null)
            {
                if (dbConnection.State == System.Data.ConnectionState.Closed)
                {
                    dbConnection.Open();
                }
                if (dbConnection.State == System.Data.ConnectionState.Broken)
                {
                    dbConnection.Close();
                    dbConnection.Open();
                }
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT * FROM [adis].[ADIS_USER] WHERE USER_GU=@identifier AND USER_BINDING_TYPE=@bindingMachineName;");
                    DbCommand cmd = dbConnection.CreateCommand();
                    cmd.Connection = dbConnection;
                    System.Diagnostics.Debug.WriteLine("Running SQL : " + sb.ToString());
                    cmd.CommandText = sb.ToString();
                    cmd.Parameters.Add(dbProvider.NewParameter("@bindingMachineName", binding.MachineName));
                    cmd.Parameters.Add(dbProvider.NewParameter("@identifier", identifier));
                    var reader = cmd.ExecuteReader();
                    List<User> users = new List<User>();
                    var rowReader = new DbRowReader(reader);
                    reader.Read();
                    var user = GetUser(reader, rowReader);

                    return user;


                }
                return null;
            }
            throw new Exception("Unable to query user data");
        }

        public User GetByToken(object token)
        {
            throw new NotImplementedException();
        }

        public void Register(User user, string password = null)
        {
             DbConnection dbConnection = null;
            if (dbProvider == null || dbConnection == null)
            {
                dbConnection = dbProvider.NewConnection(connectionString);
                dbConnection.Open();
            }
            if (dbProvider != null)
            {
                if (dbConnection.State == System.Data.ConnectionState.Closed)
                {
                    dbConnection.Open();
                }
                if (dbConnection.State == System.Data.ConnectionState.Broken)
                {
                    dbConnection.Close();
                    dbConnection.Open();
                }
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT * FROM [adis].[ADIS_USER] WHERE (USER_GU=@identifier OR EMAIL=@email OR LOGIN_NAME=@username);");
                    DbCommand cmd = dbConnection.CreateCommand();
                    cmd.Connection = dbConnection;
                    System.Diagnostics.Debug.WriteLine("Running SQL : " + sb.ToString());
                    cmd.CommandText = sb.ToString();
                    cmd.Parameters.Add(dbProvider.NewParameter("@bindingMachineName", binding.MachineName));
                    cmd.Parameters.Add(dbProvider.NewParameter("@identifier", user.UserID));
                    cmd.Parameters.Add(dbProvider.NewParameter("@email", user.Email));
                    cmd.Parameters.Add(dbProvider.NewParameter("@username", user.LoginName));
                    var reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        throw new Exception("That user already exists in the data set. Identifier, email, and login name must be unqiue.");
                    }

                    var trans = dbConnection.BeginTransaction(System.Data.IsolationLevel.Serializable);
                    try
                    {
                        sb.Clear();
                        sb.Append("INSERT INTO [adis].[ADIS_USER]");
                        sb.Append("(USER_GU,LOGIN_NAME,PASSWORD,PASSWORD_SALT,IS_ADMINISTRATOR,USER_TYPE");
                        sb.Append(",CHANGE_DATE,ADDED_DATE,ADDED_GU,CHANGED_GU,LAST_LOGIN,FIRST_LOGIN,EXPIRES");
                        sb.Append(",EXPIRES_DATE,NUM_LOGINS,USER_BINDING_TYPE,AUTHENTICATION_BINDING_TYPE,EMAIL)");
                        sb.Append(" VALUES (");
                        sb.Append("@identifier,@username,@password,@passwordSalt,@isAdministrator,@userType");
                        sb.Append(",@changeDate,@addedDate,@addedGu,@changeGu,@lastLogin,@firstLogin,@expires");
                        sb.Append(",@expiresDate,@numLogins,@userBindingType,@authenticationBindingType,@email");
                        System.Diagnostics.Debug.WriteLine("Running SQL : " + cmd.ToString());
                        var command = dbProvider.NewCommand(cmd.ToString(), dbConnection);
                        user.ChangedDate = DateTime.Now;
                        user.AddedDate = DateTime.Now;
                        user.NumLogins = 0;
                        user.LastLogin = null;
                        user.FirstLogin = null;
                        var salt = Crypto.CreateSalt();
                        user.PassSalt = Convert.ToBase64String(salt);
                        cmd.Parameters.Add(dbProvider.NewParameter("@identifier", user.UserID));
                        cmd.Parameters.Add(dbProvider.NewParameter("@username", user.LoginName));
                        cmd.Parameters.Add(dbProvider.NewParameter("@password", ""));
                        cmd.Parameters.Add(dbProvider.NewParameter("@passwordSalt", salt));
                        cmd.Parameters.Add(dbProvider.NewParameter("@isAdministrator", user.IsAdministrator));
                        cmd.Parameters.Add(dbProvider.NewParameter("@userType", user.UserType.MachineName));
                        cmd.Parameters.Add(dbProvider.NewParameter("@changeDate", user.ChangeID));
                        cmd.Parameters.Add(dbProvider.NewParameter("@addedDate", user.AddedDate));
                        cmd.Parameters.Add(dbProvider.NewParameter("@addedGu", user.AddedID));
                        cmd.Parameters.Add(dbProvider.NewParameter("@changeGu", user.ChangeID));
                        cmd.Parameters.Add(dbProvider.NewParameter("@lastLogin", user.LastLogin));
                        cmd.Parameters.Add(dbProvider.NewParameter("@firstLogin", user.FirstLogin));
                        cmd.Parameters.Add(dbProvider.NewParameter("@expires", user.Expires));
                        cmd.Parameters.Add(dbProvider.NewParameter("@expiresDate", user.ExpiresDate));
                        cmd.Parameters.Add(dbProvider.NewParameter("@numLogins", 0));
                        cmd.Parameters.Add(dbProvider.NewParameter("@userBindingType", user.UserBinding.MachineName));
                        cmd.Parameters.Add(dbProvider.NewParameter("@authenticationBindingType", user.AuthenticationBinding.MachineName));
                        cmd.Parameters.Add(dbProvider.NewParameter("@email", user.Email));
                        command.Transaction = trans;

                        command.ExecuteNonQuery();

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                    }


                }
            }
               
        }
    }
}
