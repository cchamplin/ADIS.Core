using ADIS.Core.Configuration.Util;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Configuration
{
    public class Configuration : DynamicObject
    {
        object instance;
        Type t;
        internal bool isDirty = false;
        internal bool deleted = false;
        internal bool added = false;
        static Dictionary<Type, List<TypeHelper.PropertyAccessor>> configItems = new Dictionary<Type, List<TypeHelper.PropertyAccessor>>();

        internal static Configuration FromInstance(object instance) {
            var result = new Configuration(instance.GetType(),instance);
            return result;
        }
        private Configuration(Type t, object instance)
        {
            this.t = t;
            if (!configItems.ContainsKey(t))
            {
                lock (configItems)
                {
                    var props = t.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    var accessors = new List<TypeHelper.PropertyAccessor>();
                    foreach (var prop in props)
                    {
                        accessors.Add(new TypeHelper.PropertyAccessor(t, prop));
                    }
                    configItems.Add(t, accessors);
                }
            }
            this.instance = instance;
        }
        public Configuration(Type t)
        {
            this.t = t;
            if (!configItems.ContainsKey(t))
            {
                lock (configItems)
                {
                    var props = t.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    var accessors = new List<TypeHelper.PropertyAccessor>();
                    foreach (var prop in props)
                    {
                        accessors.Add(new TypeHelper.PropertyAccessor(t, prop));
                    }
                    configItems.Add(t, accessors);
                }
            }
            instance = TypeHelper.GetConstructor(t)();
        }

        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            var props = configItems[t];
            var prop = props.Where(x => x.Name == binder.Name).First();
            isDirty = true;
            prop.Set(instance, value);
            Console.WriteLine("Setting property: " + binder.Name);
            return true;
        }
        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            var props = configItems[t];
            var prop = props.Where(x => x.Name == binder.Name).First();

            result = prop.Get(instance);
            return true;
        }
    }
}
