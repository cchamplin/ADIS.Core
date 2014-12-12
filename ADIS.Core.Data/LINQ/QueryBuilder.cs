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

        protected List<PropertyTreeProperty> globalSelected = new List<PropertyTreeProperty>();

        public class ComposedQuery
        {
            public List<PropertyTreeProperty> globalSelected = new List<PropertyTreeProperty>();
            public List<string> globalJoins = new List<string>();
            public List<string> globalConditions = new List<string>();
            public List<SelectQuery> unionedQueries = new List<SelectQuery>();
            public INameResolver resolver;
            public PropertyTree tableRoot;
            public int ordinal;
            public string Build()
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT ");
                builder.AppendLine();

                if (globalSelected.Count > 0)
                {

                    var prop = globalSelected[0];
                    //builder.Append(resolver.ResolveColumnAlias(prop));
                    builder.Append(prop.propertyAlias);
                    builder.AppendLine();
                    for (int x = 1; x < globalSelected.Count; x++)
                    {
                        //builder.Append("p");
                        //builder.Append(x);
                        builder.Append(",");
                        builder.Append(globalSelected[x].propertyAlias);
                        
                        builder.AppendLine();
                    }
                    builder.Append(" FROM ");
                    foreach (var select in unionedQueries)
                    {
                        builder.Append("(");
                        builder.Append(select.Build());
                        builder.Append(")");
                        builder.AppendLine(" UNION ALL ");
                        builder.AppendLine();
                    }
                }
                return builder.ToString();
            }

            internal void Compose()
            {

                VisitTreeJoins();
                //VisitTreeSelections();
               // VisitTreeConditions();
            }
            public void VisitTreeConditions()
            {
                var query = new SelectQuery();
                query.tableRoot = tableRoot;
                query.resolver = resolver;
                query.composed = this;
                foreach (var prop in tableRoot.selectedProperties)
                {
                    if (prop.objectTree != null)
                    {
                    }
                }
            }
            public void VisitTreeSelections()
            {
            }

            public void VisitTreeJoins()
            {
                var query = new SelectQuery();
                query.tableRoot = tableRoot;
                query.resolver = resolver;
                query.composed = this;
                this.unionedQueries.Add(query);
                foreach (var prop in tableRoot.selectedProperties)
                {
                    if (prop.Columnar)
                    {
                        prop.SetOrdinal(ordinal++);
                        globalSelected.Add(prop);
                        query.selected.Add(prop);
                    }
                    if (prop.objectTree != null)
                    {
                        StringBuilder sb = new StringBuilder();
                        prop.BuildJoin(sb, resolver);

                        if (!(prop.property.Relationship is ManyToOneRelationship))
                        {
                            var nquery = new SelectQuery();
                            nquery.tableRoot = tableRoot;
                            nquery.resolver = resolver;
                            nquery.composed = this;

                            if (prop.objectTree.projected)
                            {
                                globalJoins.Add(sb.ToString());
                            }
                            else
                            {
                                nquery.joins.Add(sb.ToString());
                            }
                            this.unionedQueries.Add(nquery);
                            VisitTreeJoins(nquery, prop);
                        }
                        else
                        {

                            if (prop.objectTree.projected)
                            {
                                globalJoins.Add(sb.ToString());
                            }
                            else
                            {
                                query.joins.Add(sb.ToString());
                            }
                            VisitTreeJoins(query, prop);
                        }


                        
                        
                    }
                }
                
            }
            public void VisitTreeJoins(SelectQuery query, PropertyTreeProperty treeProp)
            {
                foreach (var prop in treeProp.objectTree.selectedProperties)
                {
                    if (prop.Columnar)
                    {
                        prop.SetOrdinal(ordinal++);
                        globalSelected.Add(prop);
                        query.selected.Add(prop);
                    }
                    if (prop.objectTree != null)
                    {
                        if (prop.property.Relationship is ManyToOneRelationship)
                        {
                            StringBuilder sb = new StringBuilder();
                            prop.BuildJoin(sb, resolver);
                            if (prop.objectTree.projected)
                            {
                                globalJoins.Add(sb.ToString());
                            }
                            else
                            {
                                query.joins.Add(sb.ToString());
                            }
                            VisitTreeJoins(query, prop);
                        }
                        else
                        {
                            var nquery = new SelectQuery();
                            nquery.tableRoot = tableRoot;
                            nquery.resolver = resolver;
                            nquery.composed = this;
                            StringBuilder sb = new StringBuilder();
                            prop.BuildJoin(sb, resolver);
                            if (prop.objectTree.projected)
                            {
                                globalJoins.Add(sb.ToString());
                            }
                            else
                            {
                                nquery.joins.Add(sb.ToString());
                            }
                            VisitTreeJoins(query, prop);
                        }
                      
                        
                    }
                }
            }
        }
        public class SelectQuery
        {
            public List<PropertyTreeProperty> selected = new List<PropertyTreeProperty>();
            public List<string> joins = new List<string>();
            public List<string> conditions = new List<string>();
            public ComposedQuery composed;
            public PropertyTree tableRoot;
            public INameResolver resolver;
            public string Build()
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT ");
                builder.AppendLine();

                if (selected.Count > 0)
                {

                    var prop = selected[0];
                    int skip = prop.Ordinal;
                    for (int x = 0; x < prop.Ordinal; x++)
                    {
                        //builder.Append("p");
                        //builder.Append(x);
                        if (composed.globalSelected[x].root == tableRoot && composed.globalSelected[x].root.dbo.PrimaryKey == composed.globalSelected[x].property)
                        {
                            builder.Append(resolver.ResolveColumnAlias(composed.globalSelected[x]));

                        }
                        else
                        {

                            builder.Append("NULL AS ");
                            builder.Append(composed.globalSelected[x].propertyAlias);
                        }

                        builder.Append(",");
                        builder.AppendLine();
                    }


                    builder.Append(resolver.ResolveColumnAlias(prop));
                    builder.AppendLine();
                    for (int x = 1; x < selected.Count; x++)
                    {
                        prop = selected[x];
                        skip++;
                        if (prop.Ordinal != skip)
                        {
                            for (int y = skip; y < prop.Ordinal; y++)
                            {
                                builder.Append(",");
                                if (composed.globalSelected[x].root == tableRoot && composed.globalSelected[y].root.dbo.PrimaryKey == composed.globalSelected[y].property)
                                {
                                    builder.Append(resolver.ResolveColumnAlias(composed.globalSelected[x]));

                                }
                                else
                                {

                                    builder.Append("NULL AS ");
                                    builder.Append(composed.globalSelected[y].propertyAlias);
                                }
                                

                                builder.AppendLine();
                            }
                            skip = prop.Ordinal;
                        }
                        builder.Append(",");
                        builder.Append(resolver.ResolveColumnAlias(prop));
                        builder.AppendLine();
                    }
                    for (int x = prop.Ordinal+1; x < composed.globalSelected.Count; x++)
                    {
                        builder.Append(",");
                        if (composed.globalSelected[x].root == tableRoot && composed.globalSelected[x].root.dbo.PrimaryKey == composed.globalSelected[x].property)
                        {
                            builder.Append(resolver.ResolveColumnAlias(composed.globalSelected[x]));

                        }
                        else
                        {

                            builder.Append("NULL AS ");
                            builder.Append(composed.globalSelected[x].propertyAlias);
                        }
                       
                        builder.AppendLine();
                    }
                }
                builder.Append(" FROM ");
                builder.AppendLine();
                builder.AppendLine(resolver.ResolveObjectAlias(tableRoot));
                builder.AppendLine();
                if (joins.Count > 0)
                {
                    for (int x = 0; x < joins.Count; x++)
                    {
                        builder.Append(joins[x]);
                        builder.AppendLine();
                    }
                }
                if (composed.globalJoins.Count > 0)
                {
                    for (int x = 0; x < composed.globalJoins.Count; x++)
                    {
                        builder.Append(composed.globalJoins[x]);
                        builder.AppendLine();
                    }
                }
                //builder.Append("WHERE");
                if (conditions.Count > 0)
                {
                    for (int x = 0; x < conditions.Count; x++)
                    {
                        builder.Append(conditions[x]);
                        builder.AppendLine();
                    }
                }
                if (composed.globalConditions.Count > 0)
                {
                    for (int x = 0; x < composed.globalConditions.Count; x++)
                    {
                        builder.Append(composed.globalConditions[x]);
                        builder.AppendLine();
                    }
                }
                return builder.ToString();
            }

        }

        public int VisitTree(StringBuilder builder, PropertyTree tree, int ordinal = 0)
        {
            StringBuilder joins = new StringBuilder();
            builder.Append("SELECT ");
            builder.AppendLine();
            var subs = new List<SubSelect>();
            foreach (var prop in tree.selectedProperties)
            {
                if (prop.Columnar)
                {
                    if (ordinal > 0)
                        builder.Append(", ");
                    builder.Append(_resolver.ResolveColumnAlias(prop));
                    prop.SetOrdinal(ordinal++);
                    globalSelected.Add(prop);
                    builder.AppendLine();
                }
                else
                {
                    if (prop.objectTree != null)
                    {
                        
                        VisitJoins(subs, joins, prop,ordinal);
                    }
                }
            }
            builder.Append(" FROM ");
            builder.AppendLine();
            builder.Append(_resolver.ResolveObjectAlias(tree));
            builder.AppendLine();
            builder.Append(joins.ToString());
            builder.AppendLine();
            foreach (var sub in subs)
            {
                builder.Append(sub.builder.ToString());
                builder.AppendLine();
            }
            return ordinal;
        }
        public int VisitJoins(List<SubSelect> queries, StringBuilder builder, PropertyTreeProperty joinProp, int ordinal)
        {

            if (joinProp.objectTree != null)
            {
                if (joinProp.property.Relationship is ManyToOneRelationship)
                {

                    joinProp.BuildJoin(builder, _resolver);
                    foreach (var prop in joinProp.objectTree.selectedProperties)
                    {
                        if (prop.Columnar)
                        {
                            if (ordinal > 0)
                                builder.Append(", ");
                            builder.Append(_resolver.ResolveColumnAlias(joinProp));
                            joinProp.SetOrdinal(ordinal++);
                            globalSelected.Add(joinProp);
                            builder.AppendLine();
                        }
                        else
                        {
                            if (prop.objectTree != null)
                            {
                                if (prop.property.Relationship is ManyToOneRelationship)
                                {
                                    ordinal = VisitJoins(queries, builder, prop, ordinal);
                                }
                                else
                                {
                                    SubSelect sub = new SubSelect();
                                    ordinal = VisitTree(sub.builder, prop.objectTree, ordinal);
                                    queries.Add(sub);
                                }
                            }
                        }
                    }
                }
                else
                {
                    SubSelect sub = new SubSelect();
                    ordinal = VisitTree(sub.builder, joinProp.objectTree, ordinal);
                    queries.Add(sub);
                }
            }
            return ordinal;
            /*foreach (var prop in tree.selectedProperties)
            {
                if (!prop.Columnar)
                {
                    if (prop.objectTree != null)
                    {
                        if (prop.property.Relationship is ManyToOneRelationship)
                        {
                            prop.BuildJoin(builder, _resolver);
                            VisitJoins(queries, builder, prop, prop.objectTree, ordinal);
                        }
                        else
                        {
                            SubSelect sub = new SubSelect();
                            VisitTree(sub.builder, prop.objectTree, ordinal);
                            queries.Add(sub);
                        }
                    }
                }
            }*/
        }

        public class SubSelect
        {
            public StringBuilder builder;
            public SubSelect(StringBuilder builder)
            {
                this.builder = builder;
            }
            public SubSelect()
            {
                this.builder = new StringBuilder();
            }
        }

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
                    if (!_resolver.PropertyTree.selectedProperties.Contains(child))
                    {
                        _resolver.PropertyTree.selectedProperties.Add(child);
                    }
                    if (child.Columnar)
                    {
                       
                        SelectParameter += _resolver.ResolveColumnAlias(child);
                    }
                    for (int x = 1; x < _resolver.PropertyTree.children.Count; x++)
                    {
                        child = _resolver.PropertyTree.children[x];
                        if (!_resolver.PropertyTree.selectedProperties.Contains(child))
                        {
                            _resolver.PropertyTree.selectedProperties.Add(child);
                        }
                        if (child.Columnar)
                        {
                           
                            SelectParameter += "," + _resolver.ResolveColumnAlias(child);
                        }
                    }

                    SelectParameter = BuildParameter(StringConstants.SelectParameter, SelectParameter);
                }
                else {
                    throw new Exception("No columns selected");
                }
            }

            ComposedQuery comp = new ComposedQuery();
            comp.resolver = _resolver;
            comp.tableRoot = _resolver.PropertyTree;
            comp.globalConditions.Add(WhereParameter);
            comp.Compose();
            System.Diagnostics.Debug.WriteLine(comp.Build());
           // StringBuilder data = new StringBuilder();
            //int ordinal = VisitTree(data, _resolver.PropertyTree, 0);
            //System.Diagnostics.Debug.WriteLine(data.ToString());

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
