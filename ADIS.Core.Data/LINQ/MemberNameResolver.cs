using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ADIS.Core.Data.Util;

namespace ADIS.Core.Data.LINQ
{
    /// <summary>
    /// Resolves the name or alias for a type member based on the serialization attribute.
    /// </summary>
    public class MemberNameResolver : INameResolver
    {
        private static readonly ConcurrentDictionary<MemberInfo, string> KnownMemberNames = new ConcurrentDictionary<MemberInfo, string>();
        private static readonly ConcurrentDictionary<MemberInfo, string> KnownMemberColumns = new ConcurrentDictionary<MemberInfo, string>();
        private static readonly ConcurrentDictionary<string, MemberInfo> KnownAliasNames = new ConcurrentDictionary<string, MemberInfo>();
        private IDataBound rootObject;
        private PropertyTree tree;
        private int tableCount;
        private int propertyCount;
        public MemberNameResolver()
        {
            
        }


        /// <summary>
        /// Returns the resolved <see cref="MemberInfo"/> for an alias.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> the alias relates to.</param>
        /// <param name="alias">The name of the alias.</param>
        /// <returns>The <see cref="MemberInfo"/> which is aliased.</returns>
        public MemberInfo ResolveAlias(Type type, string alias)
        {
            var key = type.AssemblyQualifiedName + alias;
            return KnownAliasNames.GetOrAdd(key, s => ResolveAliasInternal(type, alias));
        }

        /// <summary>
        /// Returns the resolved name for the <see cref="MemberInfo"/>.
        /// </summary>
        /// <param name="member">The <see cref="MemberInfo"/> to resolve the name of.</param>
        /// <returns>The resolved name.</returns>
        public string ResolveName(MemberInfo member)
        {
            var result = KnownMemberNames.GetOrAdd(member, ResolveNameInternal);


            return result;
        }
        public void SetRoot(Type t,string name)
        {
            if (typeof(IDataBound).IsAssignableFrom(t))
            {
                if (rootObject != null)
                    throw new Exception("Root already set");

                var constructor = TypeHelper.GetConstructor(t);
                rootObject = constructor() as IDataBound;
                tree = new PropertyTree(rootObject, t.Name, name,"t"+tableCount++);
                foreach (var prop in rootObject.PropertyList)
                {
                    if (prop is DataBoundProperty)
                    {
                        var propItem = new PropertyTreeProperty(tree, prop as DataBoundProperty);
                        propItem.propertyAlias = "p" + propertyCount++;
                        tree.children.Add(propItem);
                    }
                }
             
            }
            else
            {
                throw new Exception("Query builder only providers functionality for DataBoundTypes");
            }
        }
        public PropertyTreeProperty ResolveDataProperty(List<string> objects)
        {
            if (tree != null)
            {
                if (objects.Count == 2)
                {
                    return tree.children.Where(x => x.property.Name == objects[1]).First();
                }
                else
                {
                    var searchTree = tree;
                    PropertyTreeProperty prop = null;
                    for (int x = 1; x < objects.Count; x++)
                    {
                        prop = searchTree.children.Where(p => p.property.Name == objects[x]).First();
                        if (prop.Columnar == false)
                        {
                            if (prop.objectTree == null)
                            {
                                bool newItem;
                                if (!searchTree.selectedProperties.Contains(prop))
                                    searchTree.selectedProperties.Add(prop);
                                searchTree = this.ResolveObject(prop, prop.property.Name, out newItem);
                                searchTree.projected = true;
                            }
                            else
                            {
                                if (!searchTree.selectedProperties.Contains(prop))
                                    searchTree.selectedProperties.Add(prop);
                                prop.objectTree.projected = true;
                                searchTree = prop.objectTree;
                            }
                        }
                        else
                        {
                            if (x < objects.Count - 1)
                            {
                                throw new Exception("Query engine does not support comparitors on non-DataBoundProperty object members");
                            }
                        }
                    }
                    return prop;
                }
                
            }
            throw new Exception("Invalid data binding");
        }
        public string ResolveColumn(MemberInfo member)
        {
            var result = KnownMemberColumns.GetOrAdd(member, ResolveColumnInternal);


            return result;
        }


        private static MemberInfo ResolveAliasInternal(Type type, string alias)
        {

            var member = GetMembers(type)
                .Select(
                    x =>
                    {
                        if (HasAliasAttribute(alias, x))
                        {
                            return x.MemberType == MemberTypes.Field
                                ? CheckFrontingProperty(x)
                                : x;
                        }

                        if (x.Name == alias)
                        {
                            return x;
                        }

                        return null;
                    })
                .FirstOrDefault(x => x != null);

            return member;
        }

        private static bool HasAliasAttribute(string alias, MemberInfo member)
        {

            var attributes = member.GetCustomAttributes(true);
            var dataMember = attributes.OfType<DataMemberAttribute>()
                .FirstOrDefault();
            if (dataMember != null && dataMember.Name == alias)
            {
                return true;
            }

            var xmlElement = attributes.OfType<XmlElementAttribute>()
                .FirstOrDefault();
            if (xmlElement != null && xmlElement.ElementName == alias)
            {
                return true;
            }

            var xmlAttribute = attributes.OfType<XmlAttributeAttribute>()
                .FirstOrDefault();
            if (xmlAttribute != null && xmlAttribute.AttributeName == alias)
            {
                return true;
            }
            return false;
        }

        private static MemberInfo CheckFrontingProperty(MemberInfo field)
        {

            var declaringType = field.DeclaringType;
            var correspondingProperty = declaringType.GetProperties()

.FirstOrDefault(x => string.Equals(x.Name, field.Name.Replace("_", string.Empty), StringComparison.OrdinalIgnoreCase));

            return correspondingProperty ?? field;
        }

        private static IEnumerable<MemberInfo> GetMembers(Type type)
        {


            if (type.IsInterface)
            {
                var propertyInfos = new List<MemberInfo>();

                var considered = new List<Type>();
                var queue = new Queue<Type>();
                considered.Add(type);
                queue.Enqueue(type);
                while (queue.Count > 0)
                {
                    var subType = queue.Dequeue();
                    foreach (var subInterface in subType.GetInterfaces()
                        .Where(x => !considered.Contains(x)))
                    {
                        considered.Add(subInterface);
                        queue.Enqueue(subInterface);
                    }

                    var typeProperties = subType.GetMembers(
                        BindingFlags.FlattenHierarchy
                        | BindingFlags.Public
                        | BindingFlags.Instance);

                    var newPropertyInfos = typeProperties
                        .Where(x => !propertyInfos.Contains(x));

                    propertyInfos.InsertRange(0, newPropertyInfos);
                }

                return propertyInfos.ToArray();
            }

            var members = type.GetMembers(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            return members;
        }

        private static string ResolveNameInternal(MemberInfo member)
        {

            var dataMember = member.GetCustomAttributes(typeof(DataMemberAttribute), true)
                .OfType<DataMemberAttribute>()
                .FirstOrDefault();

            if (dataMember != null && dataMember.Name != null)
            {
                return dataMember.Name;
            }

            var xmlElement = member.GetCustomAttributes(typeof(XmlElementAttribute), true)
                .OfType<XmlElementAttribute>()
                .FirstOrDefault();

            if (xmlElement != null && xmlElement.ElementName != null)
            {
                return xmlElement.ElementName;
            }

            var xmlAttribute = member.GetCustomAttributes(typeof(XmlAttributeAttribute), true)
                .OfType<XmlAttributeAttribute>()
                .FirstOrDefault();

            if (xmlAttribute != null && xmlAttribute.AttributeName != null)
            {
                return xmlAttribute.AttributeName;
            }

            return member.Name;
        }


        private static string ResolveColumnInternal(MemberInfo member)
        {

            var dataMember = member.GetCustomAttributes(typeof(DataMember), true)
                .OfType<DataMember>()
                .FirstOrDefault();

            if (dataMember != null && dataMember.columnName != null)
            {
                return dataMember.columnName;
            }
            return member.Name;

          
        }
        public string NextTableAlias()
        {
            return "t" + tableCount++;
        }


        public string ResolveObjectAlias(PropertyTree obj)
        {
            return obj.dbo.Schema + "." + obj.dbo.TableName + " AS " + obj.objectAlias;
        }

        public string ResolveColumnAlias(PropertyTreeProperty member)
        {
            return member.root.objectAlias + "." + member.property.ColumnName + " AS " + member.propertyAlias;
        }

        public string ResolveColumn(PropertyTreeProperty member)
        {
            return member.root.objectAlias + "." + member.property.ColumnName;
        }


        public PropertyTree PropertyTree
        {
            get { return this.tree; }
        }


        public PropertyTree ResolveObject(PropertyTreeProperty member, string name, out bool newlyResolved)
        {
            if (member.objectTree != null)
            {
                newlyResolved = false;
                return member.objectTree;
            }
            newlyResolved = true;
            Type t = member.property.Type;
            if (t.IsGenericType)
            {
                t = t.GetGenericArguments()[0];
            }
            if (typeof(IDataBound).IsAssignableFrom(t))
            {
                var constructor = TypeHelper.GetConstructor(t);
                var obj = constructor() as IDataBound;
                var newTree = new PropertyTree(obj, t.Name, name, "t" + tableCount++);
                foreach (var prop in obj.PropertyList)
                {
                    if (prop is DataBoundProperty)
                    {
                        var propItem = new PropertyTreeProperty(newTree, prop as DataBoundProperty);
                        propItem.propertyAlias = "p" + propertyCount++;
                        newTree.children.Add(propItem);
                    }
                }
                member.objectTree = newTree;
                return newTree;

            }
            else
            {
                throw new Exception("Query builder only providers functionality for DataBoundTypes");
            }
        }
    }
}
