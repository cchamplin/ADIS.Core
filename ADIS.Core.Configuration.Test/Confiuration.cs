using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADIS.Core.Configuration.Test
{
    [TestClass]
    public class Confiuration
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
