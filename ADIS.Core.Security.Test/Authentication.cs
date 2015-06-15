using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ADIS.Core.Configuration;
using ADIS.Services;
using ADIS.Core.ComponentServices.Services;
using ADIS.Core.ComponentServices;
using FastSerialize;
using System.Net;
using System.Text;
using System.IO;
using System.Diagnostics;
namespace ADIS.Core.Security.Test
{
    [TestClass]
    public class Authentication
    {
        private void SetupServer()
        {
            var cm = ConfigurationManager.Current;
            var host = new HostedAppHost();
            var cs = ADIS.Core.ComponentServices.ComponentServices.Fetch("Services");
            var router = cs.Resolve<IServiceRouter>();

            var sp = ADIS.Core.ComponentServices.ComponentServices.Fetch("Security");
            var secProviders = sp.Resolve<ISecurityProviders>();

            var userProvider = new SqlUserProvider(new SqlUserBinding());

            var user = new User()
            {
                AddedDate = DateTime.Now,
                AddedID = Guid.Empty,
                ChangedDate = DateTime.Now,
                ChangeID = Guid.Empty,
                Email = "test@test.com",
                Expires = false,
                ExpiresDate = null,
                IsAdministrator = false,
                LoginName = "TestUser",
                UserType = new SystemUserType()
            };

            //userProvider.Register(user, "test");
            secProviders.RegisterAuthenticationProvider(new SqlAuthenticationProvider(new SqlAuthenticationBinding()));
            secProviders.RegisterUserProvider(userProvider);


            router.Add("/auth/<userbinding>/<authenticationbinding>", new AuthenticationServiceHandler());
            host.Initialize();
            host.Start("http://localhost:82/");



            Console.WriteLine("Started listening");
        }

        [TestMethod]
        public void TestSqlAuthentication()
        {
            SetupServer();
            Stopwatch sw = new Stopwatch();
            
            var sr = new Serializer(typeof(FastSerialize.JsonSerializerGeneric));
            var request = WebRequest.Create("http://localhost:82/auth/sql/sql");
            request.Method = "POST";
            var postData = "{ \"Username\":\"TestUser\",\"Password\":\"test\" }";
            var encoded = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/json";
            request.ContentLength = encoded.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(encoded, 0, encoded.Length);
            }
            sw.Start();
            var response = (HttpWebResponse)request.GetResponse();
            sw.Stop();
           
            var data = new StreamReader(response.GetResponseStream()).ReadToEnd();
            System.Diagnostics.Debug.WriteLine("Got Response: " + data + " in " + sw.Elapsed.TotalMilliseconds);
        }
        [TestMethod]
        public void TestSqlAuthenticationDefaultAuthentication()
        {
            SetupServer();
            var sr = new Serializer(typeof(FastSerialize.JsonSerializerGeneric));
            var request = WebRequest.Create("http://localhost:82/auth/sql");
            request.Method = "POST";
            var postData = "{ \"Username\":\"TestUser\",\"Password\":\"test\" }";
            var encoded = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/json";
            request.ContentLength = encoded.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(encoded, 0, encoded.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            var data = new StreamReader(response.GetResponseStream()).ReadToEnd();
            System.Diagnostics.Debug.WriteLine("Got Response: " + data);
        }
        [TestMethod]
        public void TestSqlAuthenticationDefaultUserProvider()
        {
            SetupServer();
            var sr = new Serializer(typeof(FastSerialize.JsonSerializerGeneric));
            var request = WebRequest.Create("http://localhost:82/auth/default/sql");
            request.Method = "POST";
            var postData = "{ \"Username\":\"TestUser\",\"Password\":\"test\" }";
            var encoded = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/json";
            request.ContentLength = encoded.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(encoded, 0, encoded.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            var data = new StreamReader(response.GetResponseStream()).ReadToEnd();
            System.Diagnostics.Debug.WriteLine("Got Response: " + data);
        }
        [TestMethod]
        public void TestSqlAuthenticationDefault()
        {
            SetupServer();
            var sr = new Serializer(typeof(FastSerialize.JsonSerializerGeneric));
            var request = WebRequest.Create("http://localhost:82/auth/");
            request.Method = "POST";
            var postData = "{ \"Username\":\"TestUser\",\"Password\":\"test\" }";
            var encoded = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/json";
            request.ContentLength = encoded.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(encoded, 0, encoded.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            var data = new StreamReader(response.GetResponseStream()).ReadToEnd();
            System.Diagnostics.Debug.WriteLine("Got Response: " + data);
        }
    }
}
