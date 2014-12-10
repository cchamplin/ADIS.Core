using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data;
using ADIS.Core.Data.Util;

namespace ADIS.Core.Data.LINQ
{
    internal class QueryBuilder
    {
        INameResolver _resolver;
        public QueryBuilder(INameResolver resolver, Type sourceType)
        {
            _resolver = resolver;
            OrderByParameter = new List<string>();
            SourceType = sourceType;
        }



        public Type SourceType { get; private set; }

        public Type DataType { get; set; }
        public MemberInfo ResultProperty { get; set; }

        public string WhereParameter { get; set; }

        public IList<string> OrderByParameter { get; private set; }

        public string SelectParameter { get; set; }

        public string SkipParameter { get; set; }

        public string FromParameter { get; set; }

        public string JoinParameter { get; set; }

        public string TakeParameter { get; set; }

        public string ExpandParameter { get; set; }

        public string TopParameter { get; set; }

        public string GetFullQuery()
        {
            var parameters = new List<string>();
            if (!string.IsNullOrWhiteSpace(WhereParameter))
            {

                WhereParameter = BuildParameter(StringConstants.WhereParameter, WhereParameter);
            }

            if (!string.IsNullOrWhiteSpace(SelectParameter))
            {

                if (!string.IsNullOrWhiteSpace(TopParameter))
                {
                    SelectParameter = BuildParameter(StringConstants.SelectParameter, BuildParameter(TopParameter, SelectParameter));
                }
                else
                {
                    SelectParameter = BuildParameter(StringConstants.SelectParameter, SelectParameter);
                }
            }
            else
            {
                if (_resolver.PropertyTree.children.Count > 0)
                {
                    var child = _resolver.PropertyTree.children[0];
                     _resolver.PropertyTree.selectedProperties.Add(child);
                     SelectParameter += _resolver.ResolveColumnAlias(child);

                    for (int x = 1; x < _resolver.PropertyTree.children.Count; x++)
                    {
                        child = _resolver.PropertyTree.children[x];
                        _resolver.PropertyTree.selectedProperties.Add(child);
                        SelectParameter += "," + _resolver.ResolveColumnAlias(child);
                    }

                    SelectParameter = BuildParameter(StringConstants.SelectParameter, SelectParameter);
                }
                else {
                    throw new Exception("No columns selected");
                }
            }
            if (string.IsNullOrWhiteSpace(FromParameter))
            {

                
                    FromParameter = _resolver.ResolveObjectAlias(_resolver.PropertyTree);
                    FromParameter = " " + BuildParameter(StringConstants.FromParameter, FromParameter) + " ";
            }

            /*if (!string.IsNullOrWhiteSpace(SkipParameter))
            {
                parameters.Add(BuildParameter(StringConstants.SkipParameter, SkipParameter));
            }*/

            if (!string.IsNullOrWhiteSpace(TakeParameter))
            {
                parameters.Add(BuildParameter(StringConstants.TopParameter, TakeParameter));
            }

            if (OrderByParameter.Any())
            {
                parameters.Add(BuildParameter(StringConstants.OrderByParameter, string.Join(",", OrderByParameter)));
            }

           /* if (!string.IsNullOrWhiteSpace(ExpandParameter))
            {
                parameters.Add(BuildParameter(StringConstants.ExpandParameter, ExpandParameter));
            }*/

            //var builder = new UriBuilder(_serviceBase);
            //builder.Path = (string.IsNullOrEmpty(builder.Path) ? string.Empty : "/") + string.Join("/", parameters);

            // var resultUri = builder.Uri;

            StringBuilder result = new StringBuilder();

           
            result.Append(SelectParameter);
                        
            result.Append(FromParameter);

            if (!string.IsNullOrWhiteSpace(JoinParameter))
            {
                result.Append(JoinParameter);
            }
            if (!string.IsNullOrWhiteSpace(ExpandParameter))
            {
                result.Append(ExpandParameter);
            }

            if (!string.IsNullOrWhiteSpace(WhereParameter))
            {
                result.Append(WhereParameter);
            }

            return result.ToString();
        }

        private static string BuildParameter(string name, string value)
        {
            return name + " " + value;
        }
        static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

    }
}
