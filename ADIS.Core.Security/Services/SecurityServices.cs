﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.ComponentServices.Services;
using ADIS.Core.ComponentServices.Database;
using ADIS.Core.ComponentServices;
using System.Data.Common;
using ADIS.Core.Configuration;
namespace ADIS.Core.Security
{
    public class SecurityServices
    {
        public SecurityServices()
        {
            var cs = ComponentServices.ComponentServices.Fetch("Services");
            var router = cs.Resolve<IServiceRouter>();
            var dataServices = ComponentServices.ComponentServices.Fetch("Data");
               IDatabaseProvider dbProvider = dataServices.Resolve<IDatabaseProvider>();
             var strings = ConfigurationManager.Current.BindAll<ConnectionStringConfigRecord>();
            var connectionString = strings.Where(x => x.Name == "Default").First().ConnectionString;

            router.Add("users",new UserServiceHandler(dbProvider, connectionString));
        }
        public class UserServiceHandler : RestServiceHandler {
            protected string connectionString;
            protected IDatabaseProvider dbProvider;
            public UserServiceHandler(IDatabaseProvider dbProvider, string connectionString) {
            }
            public override object Get()
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
                        sb.Append("SELECT * FROM [adis].[ADIS_USER];");
                        DbCommand cmd = dbConnection.CreateCommand();
                        cmd.Connection = dbConnection;
                        System.Diagnostics.Debug.WriteLine("Running SQL : " + sb.ToString());
                        cmd.CommandText = sb.ToString();
                        var reader = cmd.ExecuteReader();
                        List<User> users = new List<User>();
                        var rowReader = new DbRowReader(reader);
                        while (reader.Read())
                        {
                            object[] data = new object[reader.FieldCount];
                            reader.GetValues(data);
                            users.Add(new User()
                            {

                            });
                           
                        }
                    }
                }
                throw new Exception("Unable to query user data");
            }
        }
    }
}
