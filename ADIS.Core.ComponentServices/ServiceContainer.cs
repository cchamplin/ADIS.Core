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
        public void RegisterOrReplace(object service)
        {
            lock (services)
            {
                if (services.ContainsKey(service.GetType()))
                {
                    services[service.GetType()] = service;
                }
                else
                {
                    services.Add(service.GetType(), service);
                }
            }
        }
        public bool ServiceTypeRegistered(Type t)
        {
            return services.ContainsKey(t);
        }
        public bool ServiceRegistered(object service)
        {
            if (services.ContainsKey(service.GetType()))
            {
                return services[service.GetType()] == service;
            }
            return false;
        }
        
        public void Register(object service)
        {
            lock (services)
            {
                if (!services.ContainsKey(service.GetType()))
                {
                    services.Add(service.GetType(), service);
                    return;
                }
            }
            throw new Exception("Service already registered");
        }
    }
}
