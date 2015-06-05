using System;
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
       
    }
}
