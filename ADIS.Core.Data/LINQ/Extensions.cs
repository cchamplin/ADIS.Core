using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.LINQ
{
    /// <summary>
    /// Defines extension methods on IQueryables.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Creates a task to execute the query.
        /// </summary>
        /// <param name="queryable">The <see cref="IQueryable{T}"/> to execute.</param>
        /// <typeparam name="T">The generic type parameter.</typeparam>
        /// <returns>A task returning the query result.</returns>
        public static Task<IEnumerable<T>> ExecuteAsync<T>(this IQueryable<T> queryable)
        {

            return Task.Factory.StartNew(() => queryable.ToArray().AsEnumerable());
        }

        public static Tuple<Type, string> GetColumnFromName(this INameResolver memberNameResolver, MemberInfo name, Type sourceType)
        {
 
            var source = sourceType.GetProperties()
                .Select(x => new { Original = x, Name = memberNameResolver.ResolveColumn(x) })
                .FirstOrDefault(x => x.Original.Name == name.Name);

            return source != null
                       ? new Tuple<Type, string>(GetMemberType(source.Original), source.Name)
                       : new Tuple<Type, string>(GetMemberType(name), name.Name);
        }

        public static Tuple<Type, string> GetNameFromAlias(this INameResolver memberNameResolver, MemberInfo alias, Type sourceType)
        {


            var source = sourceType.GetMembers()
                .Select(x => new { Original = x, Name = memberNameResolver.ResolveName(x) })
                .FirstOrDefault(x => x.Original.Name == alias.Name);

            return source != null
                       ? new Tuple<Type, string>(GetMemberType(source.Original), source.Name)
                       : new Tuple<Type, string>(GetMemberType(alias), alias.Name);
        }

        private static Type GetMemberType(MemberInfo member)
        {


            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
                default:
                    throw new InvalidOperationException(member.MemberType + " is not resolvable");
            }
        }

        /// <summary>
        /// Expands the specified source.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="paths">The paths to expand in the format "Child1, Child2/GrandChild2".</param>
        /// <returns></returns>
        /*public static IQueryable<TSource> Expand<TSource>(this IQueryable<TSource> source, string paths)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (!(source is SqlQueryable<TSource>))
            {
                return source;
            }

            return source.Provider.CreateQuery<TSource>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(TSource) }), new[] { source.Expression, Expression.Constant(paths) }));
        }*/
        private static IQueryable<TSource> Expand<TSource>(this IQueryable<TSource> source, List<Expansion> expansions)
        {
            return default(IQueryable<TSource>);
        }
        public static IQueryable<TSource> Expand<TSource>(this IQueryable<TSource> source, string paths)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (!(source is SqlQueryable<TSource>))
            {
                return source;
            }
            Expression result = source.Expression;
            var expansions = paths.Split(new char[] { ',' });
            List<Expansion> composed = new List<Expansion>();
            foreach (var expansion in expansions)
            {
                var parts = expansion.Split(new char[] { '/' });
                Type[] types = new Type[parts.Length];
                PropertyInfo[] members = new PropertyInfo[parts.Length];
                var nextType = typeof(TSource);
                for (int x = 0; x < parts.Length; x++)
                {
                    types[x] = nextType;
                    members[x] = nextType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(n => n.Name == parts[x]).First();
                    nextType = members[x].PropertyType;
                   
                }
                composed.Add(new Expansion() { Type = types, Member = members });
            }
            var methodInfo = typeof(QueryableExtensions).GetMethod("Expand", BindingFlags.Static | BindingFlags.NonPublic);
            return source.Provider.CreateQuery<TSource>(Expression.Call(null, (methodInfo).MakeGenericMethod(new[] { typeof(TSource) }), new[] { source.Expression, Expression.Constant(composed) }));
        }
    }
}
