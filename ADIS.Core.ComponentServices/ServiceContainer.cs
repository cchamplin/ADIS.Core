using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.ComponentServices
{
    public class ServiceContainer
    {
        protected Dictionary<Type, object> services;
        internal ServiceContainer()
        {
            services = new Dictionary<Type, object>();
        }
        public T Resolve<T>()
        {
            if (services.ContainsKey(typeof(T)))
            {
                return (T)services[typeof(T)];
            }
            throw new Exception("Unregistered service type requested");
        }
        public void RegisterOrReplace(Type @interface, object service)
        {
            lock (services)
            {
                if (services.ContainsKey(@interface))
                {
                    services[@interface] = service;
                }
                else
                {
                    services.Add(@interface, service);
                }
            }
        }
        public void Register(Type @interface, object service)
        {
            lock (services)
            {
                if (!services.ContainsKey(@interface))
                {
                    services.Add(@interface, service);
                    return;
                }
            }
            throw new Exception("Service already registered");
        }
        public void Unregister(Type @interface)
        {
            lock (services)
            {
                if (services.ContainsKey(@interface))
                {
                    services.Remove(@interface);
                    return;
                }
            }
        }
        public bool ServiceTypeRegistered(Type @interface)
        {
            return services.ContainsKey(@interface);
        }
        public bool ServiceRegistered(object service)
        {
            if (services.ContainsKey(service.GetType()))
            {
                return services[service.GetType()] == service;
            }
            return false;
        }
        
        
    }
}
