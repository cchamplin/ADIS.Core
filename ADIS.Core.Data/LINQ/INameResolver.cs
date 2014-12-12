using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.LINQ
{
    public interface INameResolver
    {
        /// <summary>
        /// Returns the resolved name for the <see cref="MemberInfo"/>.
        /// </summary>
        /// <param name="member">The <see cref="MemberInfo"/> to resolve the name of.</param>
        /// <returns>The resolved name.</returns>
        string ResolveName(MemberInfo member);
        string ResolveColumn(MemberInfo member);
        string ResolveObjectAlias(PropertyTree tree);
        string ResolveColumnAlias(PropertyTreeProperty member);
        string ResolveColumn(PropertyTreeProperty member);
        PropertyTree ResolveObject(PropertyTreeProperty member, string name, out bool newlyResolved);
        PropertyTreeProperty ResolveDataProperty(List<string> objects);
        string NextTableAlias();
        void SetRoot(Type t, string name);
        PropertyTree PropertyTree
        {
            get;
        }
        /// <summary>
        /// Returns the resolved <see cref="MemberInfo"/> for an alias.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> the alias relates to.</param>
        /// <param name="alias">The name of the alias.</param>
        /// <returns>The <see cref="MemberInfo"/> which is aliased.</returns>
        MemberInfo ResolveAlias(Type type, string alias);
    }
    
}
