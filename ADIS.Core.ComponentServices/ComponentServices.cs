using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.ComponentServices
{
    public static class ComponentServices
    {
        private static Dictionary<string,ServiceContainer> containers;
        static ComponentServices() {
            containers = new Dictionary<string,ServiceContainer>();
        }
        public static ServiceContainer Fetch(string name)
        {
            if (!containers.ContainsKey(name))
                throw new Exception("No such container has been registered");
            return containers[name];

        }
        public static ServiceContainer Register(string name)
        {
            if (containers.ContainsKey(name))
            {
                return containers[name];
            }
            ServiceContainer container = new ServiceContainer();
            containers.Add(name,container);
            return container;
        }
    }
}
