using ADIS.Core.Configuration.Util;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using ADIS.Core.ComponentServices.Database;

namespace ADIS.Core.Configuration
{
    public class Configuration : DynamicObject
    {
        object instance;
        Type t;
        internal bool isDirty = false;
        internal bool added = false;
        static Dictionary<Type, List<TypeHelper.PropertyAccessor>> configItems = new Dictionary<Type, List<TypeHelper.PropertyAccessor>>();

        internal static Configuration FromInstance(object instance) {
            var result = new Configuration(instance.GetType(),instance);
            result.added = true;
            return result;
        }
        public void Commit(string tableName, DbConnection connection, DbTransaction transaction)
        {
            if (added)
            {
                var accessors = configItems[t];
                DbCommand cmd = connection.CreateCommand();
                cmd.Transaction = transaction;
                StringBuilder cmdString = new StringBuilder();
                cmdString.Append("INSERT INTO ");
                cmdString.Append(tableName);
                cmdString.Append(" (");
                var accessor = accessors[0];
                if (accessor.ConfigurationProperty.Primary && (Guid)accessor.Get(instance) == Guid.Empty)
                    accessor.Set(instance, Guid.NewGuid());

                cmdString.Append('[');
                cmdString.Append(accessor.ConfigurationProperty.Column);
                cmdString.Append(']');
                for (int x = 1; x < accessors.Count; x++)
                {
                    accessor = accessors[x];
                    if (accessor.ConfigurationProperty.Primary && (Guid)accessor.Get(instance) == Guid.Empty)
                        accessor.Set(instance, Guid.NewGuid());
                    cmdString.Append(", ");
                    cmdString.Append('[');
                    cmdString.Append(accessor.ConfigurationProperty.Column);
                    cmdString.Append(']');

                }
                cmdString.Append(") VALUES (");
                accessor = accessors[0];
                cmdString.Append("@p0");

                var param = cmd.CreateParameter();
                param.ParameterName = "@p0";
                param.Value = accessor.Get(instance);
                cmd.Parameters.Add(param);
                for (int x = 1; x < accessors.Count; x++)
                {
                    accessor = accessors[x];
                    cmdString.Append(", ");
                    cmdString.Append("@p");
                    cmdString.Append(x);
                    param = cmd.CreateParameter();
                    param.ParameterName = "@p"+x;
                    param.Value = accessor.Get(instance);
                    cmd.Parameters.Add(param);

                }
                cmdString.Append(");");
                System.Diagnostics.Debug.WriteLine("Running SQL : " + cmdString.ToString());
                cmd.CommandText = cmdString.ToString();
                cmd.ExecuteNonQuery();
                added = false;
            }
            if (isDirty)
            {
                var accessors = configItems[t];
                DbCommand cmd = connection.CreateCommand();
                cmd.Transaction = transaction;
                StringBuilder cmdString = new StringBuilder();
                cmdString.Append("UPDATE ");
                cmdString.Append(tableName);
                cmdString.Append(" SET");
                var accessor = accessors[0];
                cmdString.Append('[');
                cmdString.Append(accessor.ConfigurationProperty.Column);
                cmdString.Append("]=");
                cmdString.Append("@p0");
                var param = cmd.CreateParameter();
                param.ParameterName = "@p0";
                param.Value = accessor.Get(instance);
                cmd.Parameters.Add(param);
                for (int x = 1; x < accessors.Count; x++)
                {
                    accessor = accessors[x];
                    cmdString.Append(", ");
                    cmdString.Append('[');
                    cmdString.Append(accessor.ConfigurationProperty.Column);
                    cmdString.Append("]=");
                    cmdString.Append("@p");
                    cmdString.Append(x);
                    cmdString.Append(", ");

                    param = cmd.CreateParameter();
                    param.ParameterName = "@p" + x;
                    param.Value = accessor.Get(instance);
                    cmd.Parameters.Add(param);

                }
                cmdString.Append(";");

                cmd.CommandText = cmdString.ToString();
                System.Diagnostics.Debug.WriteLine("Running SQL : " + cmdString.ToString());
                cmd.ExecuteNonQuery();
                isDirty = false;
            }
        }
        private Configuration(Type t, object instance)
        {
            this.t = t;
            if (!configItems.ContainsKey(t))
            {
                lock (configItems)
                {
                    var props = t.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var accessors = new List<TypeHelper.PropertyAccessor>();
                    foreach (var prop in props)
                    {
                        var accessor = new TypeHelper.PropertyAccessor(t, prop);
                        accessors.Add(accessor);
                    }

                    configItems.Add(t, accessors);
                }
            }
            this.instance = instance;
        }
        internal Configuration(Type t)
        {
            this.t = t;
            if (!configItems.ContainsKey(t))
            {
                lock (configItems)
                {
                    var props = t.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
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

        public void ReadData(DbRowReader reader, object[] data)
        {
            var accessors = configItems[t];
            for (int x = 0; x < accessors.Count; x++)
            {
                accessors[x].Set(instance, data[x]);

            }
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
