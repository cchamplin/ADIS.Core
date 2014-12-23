using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.ComponentServices;
using System.Reflection;
using ADIS.Core.Configuration.Util;
using System.Data.Common;
using ADIS.Core.ComponentServices.Database;
namespace ADIS.Core.Configuration
{
    public class ConfigurationManager
    {
        private static ConfigurationManager instance;
        protected Dictionary<Type, WrappedConfiguration> configurations;
        protected List<WrappedConfiguration> configurationsList;
        protected FastSerialize.Serializer serializer;
        protected Task configurationSaveTask;
        protected ServiceContainer dataServices;

        protected class WrappedConfiguration
        {
            public dynamic item;
            public ConfigurationEntity configEntity;
            public string primaryKeyColumn;
            public WrappedConfiguration(dynamic item, ConfigurationEntity entity, string primaryKey)
            {
                this.item = item;
                this.configEntity = entity;
                this.primaryKeyColumn = primaryKey;
            }
        }

        private ConfigurationManager()
        {
            configurations = new Dictionary<Type, WrappedConfiguration>();
            configurationsList = new List<WrappedConfiguration>();
            serializer = new FastSerialize.Serializer(typeof(FastSerialize.JsonSerializerGeneric));
            var services = BindAll<ServiceContainerConfigRecord>(true);

            foreach (var container in services)
            {
                var cs = container as ServiceContainerConfigRecord;
                var serviceContainer = ComponentServices.ComponentServices.Register(cs.Name);
                foreach (var service in cs.Services) {

                    try
                    {
                        var assembly = Assembly.Load(service.Assembly);

                        var iface = assembly.GetTypes().Where(x => x.IsInterface && x.Name == service.Interface).FirstOrDefault();

                        if (iface == null)
                        {
                            throw new Exception("Could not locate interface: " + service.Interface);
                        }

                        var instanceType = assembly.GetTypes().Where(x => x.Name == service.Type).FirstOrDefault();
                        if (instanceType == null)
                        {
                            throw new Exception("Could not locate service type: " + service.Type);
                        }

                        var instance = TypeHelper.GetConstructor(instanceType)();

                        serviceContainer.RegisterOrReplace(iface, instance);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Failed to load service: " + service.Interface + "," + service.Assembly);
                    }
                }
                
            }
            System.Diagnostics.Debug.WriteLine("Found " + services.Count + " services");
            dataServices = ComponentServices.ComponentServices.Fetch("Data");
            configurationSaveTask = TaskExecutorFactory.Begin(SaveAll,5000,10000);

        }
        public void Empty<T>()
        {
            if (configurations.ContainsKey(typeof(T)))
            {
                var item = configurations[typeof(T)];
                if (item.item is IList<dynamic>)
                {
                    item.item.Clear();
                }
                return;
            }
            throw new Exception("Configuration entity has not been bound to");
        }
        public void SaveAll<T>()
        {
           
            if (configurations.ContainsKey(typeof(T)))
            {

                var item = configurations[typeof(T)];
                SaveConfiguration(item);
                return;
            }
            throw new Exception("Configuration entity has not been bound to");
        }

        private void SaveConfiguration(WrappedConfiguration config, IDatabaseProvider dbProvider = null, DbConnection dbConnection = null)
        {
            if (config.configEntity.Type == ConfigurationEntityType.FileBound)
            {
                try
                {
                    var serialized = serializer.Serialize(config.item);
                    using (var fs = new FileStream(config.configEntity.Name + ".json", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                    {
                        using (var sw = new StreamWriter(fs))
                        {
                            sw.Write(serialized);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Failed save config: " + config.configEntity.Name);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            else if (config.configEntity.Type == ConfigurationEntityType.DataBound)
            {
                if (dbProvider == null || dbConnection == null)
                {
                    
                    if (dataServices != null)
                    {
                        dbProvider = dataServices.Resolve<IDatabaseProvider>();
                        var strings = BindAll<ConnectionStringConfigRecord>();
                        var connectionString = strings.Where(x => x.Name == "Default").First().ConnectionString;
                        if (dbProvider != null)
                        {
                            dbConnection = dbProvider.NewConnection(connectionString);
                            dbConnection.Open();

                        }
                    }
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
                        ConfigurationItemList list = config.item as ConfigurationItemList;
                        if (list == null)
                        {
                            System.Diagnostics.Debug.WriteLine("Underlying list overwritten invalid date type");
                            return;
                        }
                        lock (list.locker)
                        {
                            if (list.removals.Count > 0)
                            {
                                StringBuilder cmd = new StringBuilder();
                                cmd.Append("DELETE FROM [");
                                cmd.Append(config.configEntity.Schema);
                                cmd.Append("].[");
                                cmd.Append(config.configEntity.TableName);
                                cmd.Append("] WHERE [");
                                cmd.Append(config.primaryKeyColumn);
                                cmd.Append("] = @id");
                                var trans = dbConnection.BeginTransaction(System.Data.IsolationLevel.Serializable);
                                try
                                {

                                    foreach (var id in list.removals)
                                    {
                                        System.Diagnostics.Debug.WriteLine("Running SQL : " + cmd.ToString() + " " + id);
                                        var command = dbProvider.NewCommand(cmd.ToString(), dbConnection);
                                        command.Transaction = trans;
                                        var param = command.CreateParameter();
                                        param.ParameterName = "@id";
                                        param.Value = id;
                                        command.ExecuteNonQuery();
                                    }
                                    trans.Commit();
                                }
                                catch (Exception ex)
                                {
                                    trans.Rollback();
                                }
                            }
                        }
                        lock (list.locker)
                        {
                            if (list.Count > 0)
                            {
                                var trans = dbConnection.BeginTransaction(System.Data.IsolationLevel.Serializable);
                                string tableAlias = "[" + config.configEntity.Schema + "].[" + config.configEntity.TableName + "]";
                                try
                                {
                                    foreach (dynamic record in list)
                                    {
                                        record.Commit(tableAlias, dbConnection, trans);
                                    }
                                    trans.Commit();
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine("Transaction failed to commit");
                                    System.Diagnostics.Debug.WriteLine(ex.Message);
                                    trans.Rollback();
                                }

                            }
                        }
                    }

                }
            }
        }

        private void SaveAll() {

            
            IDatabaseProvider dbProvider = null;
            DbConnection dbConnection = null;
            if (dataServices != null)
            {
                dbProvider = dataServices.Resolve<IDatabaseProvider>();
                var strings = BindAll<ConnectionStringConfigRecord>();
                var connectionString = strings.Where(x => x.Name == "Default").First().ConnectionString;
                if (dbProvider != null)
                {
                    dbConnection = dbProvider.NewConnection(connectionString);
                    dbConnection.Open();

                }
            }
            foreach (WrappedConfiguration item in configurationsList)
            {
                SaveConfiguration(item,dbProvider,dbConnection);
            }
        }
        public static ConfigurationManager Current
        {
            get
            {
                if (instance == null)
                {
                    instance = LoadConfiguration();
                }
                return instance;
            }
        }
        private static ConfigurationManager LoadConfiguration()
        {
            return new ConfigurationManager();
        }

        public dynamic Bind<T>() where T : new()
        {
            return configurations[typeof(T)];
        }
        public IList<dynamic> BindAll<T>(bool ignoreMissing = false) where T : new()
        {
            if (configurations.ContainsKey(typeof(T)))
            {
                return configurations[typeof(T)].item;
            }

            ConfigurationEntity ce = null;
            var attributes = typeof(T).GetCustomAttributes(false);
            foreach (var attribute in attributes)
            {
                if (attribute is ConfigurationEntity)
                {
                    ce = attribute as ConfigurationEntity;
                    break;
                }
            }
            if (ce == null)
                throw new Exception("No configuration entity type defined");

            if (ce.Type == ConfigurationEntityType.FileBound)
            {
                dynamic results = BindAllFile<T>(ce, ignoreMissing);

                var wrapped = new WrappedConfiguration(results, ce, null);
                configurations.Add(typeof(T), wrapped);
                configurationsList.Add(wrapped);
                return results;
            }
            else if (ce.Type == ConfigurationEntityType.DataBound)
            {
                var props = typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                string primaryColumn = null;

                StringBuilder sb = new StringBuilder();
                bool first = true;
                sb.Append("SELECT ");

                foreach (var prop in props)
                {
                    var attr = prop.GetCustomAttribute<ConfigurationProperty>();
                    if (attr != null)
                    {
                        if (attr.Primary)
                        {
                            primaryColumn = attr.Column;
                        }
                        if (!first)
                            sb.Append(',');
                        first = false;
                        sb.Append(attr.Column);
                    }

                }
                sb.Append(" FROM [");
                sb.Append(ce.Schema);
                sb.Append("].[");
                sb.Append(ce.TableName);
                sb.Append("];");
                var result = new ConfigurationItemList();

                IDatabaseProvider dbProvider = null;
                DbConnection dbConnection = null;
                if (dataServices != null)
                {
                    dbProvider = dataServices.Resolve<IDatabaseProvider>();
                    var strings = BindAll<ConnectionStringConfigRecord>();
                    var connectionString = strings.Where(x => x.Name == "Default").First().ConnectionString;
                    if (dbProvider != null)
                    {
                        dbConnection = dbProvider.NewConnection(connectionString);
                        dbConnection.Open();

                    }
                }
                if (dbConnection != null)
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
                        DbCommand cmd = dbConnection.CreateCommand();
                        cmd.Connection = dbConnection;
                        System.Diagnostics.Debug.WriteLine("Running SQL : " + sb.ToString());
                        cmd.CommandText = sb.ToString();
                        var reader = cmd.ExecuteReader();

                        var rowReader = new DbRowReader(reader);
                        while (reader.Read())
                        {
                            object[] data = new object[reader.FieldCount];
                            reader.GetValues(data);
                            dynamic config = new Configuration(typeof(T));
                            config.ReadData(rowReader, data);
                            result.Add(config);
                        }

                        var wrapped = new WrappedConfiguration(result, ce, primaryColumn);
                        configurations.Add(typeof(T), wrapped);
                        configurationsList.Add(wrapped);
                    }

                }



               
                return result;
            }
            return (IList<dynamic>)new ConfigurationItemList();


        }
        private IList<dynamic> BindAllFile<T>(ConfigurationEntity ce, bool ignoreMissing) where T : new()
        {
            string fileName = ce.Name + ".json";
            if (!File.Exists(fileName))
            {
                if (ignoreMissing)
                {
                    return new List<dynamic>();
                }

                throw new Exception("Unable to find configuration file: " + fileName);
            }
            List<T> data = null;
            using (var fileReader = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                data = serializer.Deserialize<List<T>>(fileReader,false);
            }
            if (data == null)
            {
                if (ignoreMissing)
                {
                    return new List<dynamic>();
                }
                throw new Exception("Configuration data could not be loaded");
            }
            List<dynamic> results = new List<dynamic>();
            foreach (var result in data)
            {
                results.Add(result);
            }
            return (IList<dynamic>)results;
        }
       
    }
}
