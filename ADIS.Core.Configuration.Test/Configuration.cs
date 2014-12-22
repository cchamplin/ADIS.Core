using System;
using ADIS.Core.Data.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADIS.Core.Configuration.Test
{
    [TestClass]
    public class Configuration
    {
        [TestMethod]
        public void TestListAdd()
        {
            var cm = ConfigurationManager.Current;
            var configItems = cm.BindAll<SimpleConfig>();

            var itemA = new SimpleConfig();
            itemA.PropA = 15;
            itemA.PropB = "Item A";

            var itemB = new SimpleConfig();
            itemB.PropA = 9;
            itemB.PropB = "Item B";


            configItems.Add(itemA);
            configItems.Add(itemB);

            Assert.AreEqual(configItems.Count,2);

        }
        [TestMethod]
        public void TestServiceComponentsAccess()
        {
            var cm = ConfigurationManager.Current;
            ITestService service = ComponentServices.ComponentServices.Fetch("Test").Resolve<ITestService>();
            service.DoTest("Merf");
        }

        [TestMethod]
        public void TestServiceComponentsAdd()
        {
            var cm = ConfigurationManager.Current;
            var configItems = cm.BindAll<ServiceContainerConfigRecord>();
            cm.Empty<ServiceContainerConfigRecord>();

            System.Diagnostics.Debug.WriteLine("After Empty: " + configItems.Count);
            var container = new ServiceContainerConfigRecord() { Name = "Test", Services = new System.Collections.Generic.List<ServiceConfigRecord>() };

            ServiceConfigRecord record = new ServiceConfigRecord() {
                Assembly = "ADIS.Core.Configuration.Test",
                Type = "TestService",
                Interface = "ITestService"
            };

            container.Services.Add(record);

            configItems.Add(container);

            cm.SaveAll<ServiceContainerConfigRecord>();
        }

        [TestMethod]
        public void TestConfigSaves()
        {
            var cm = ConfigurationManager.Current;
            var configItems = cm.BindAll<DBOConfig>(false);

            DBOConfig item = new DBOConfig();
            item.Assembly = "Test.Assembly";
            item.Name = "TestName";
            item.Route = "TestRoute";
            item.Namespace = "TestNS";
            configItems.Add(item);
            item = new DBOConfig();
            item.Assembly = "Test2.Assembly";
            item.Name = "TestName2";
            item.Route = "TestRoute2";
            item.Namespace = "TestNS2";
            configItems.Add(item);

            cm.SaveAll<DBOConfig>();
        }

        [TestMethod]
        public void TestListList()
        {
            var cm = ConfigurationManager.Current;
            var configItems = cm.BindAll<SimpleConfig>();

            var itemA = new SimpleConfig();
            itemA.PropA = 15;
            itemA.PropB = "Item A";

            var itemB = new SimpleConfig();
            itemB.PropA = 9;
            itemB.PropB = "Item B";


            configItems.Add(itemA);
            configItems.Add(itemB);

            Assert.AreEqual(configItems.Count, 2);

            foreach (dynamic item in configItems)
            {
                System.Diagnostics.Debug.WriteLine((string)item.PropB);
            }

            Assert.AreEqual(configItems[0].PropB, "Item A");
            Assert.AreEqual(configItems[1].PropB, "Item B");

        }

    }
}
