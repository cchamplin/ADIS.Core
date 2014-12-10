using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.LINQ.Writers
{
    internal class ExpansionWriter : IResolvedValueWriter
    {
        public bool Handles(Type type)
        {
            return type == typeof(List<Expansion>);
        }

        public string WriteSqlFragment(object value, INameResolver nameResolver)
        {
            StringBuilder builder = new StringBuilder();
            List<Expansion> expansions = value as List<Expansion>;
            if (nameResolver.PropertyTree != null)
            {
                foreach (var expansion in expansions) {
                    var prop = expansion.Member[0];
                    var tree = nameResolver.PropertyTree;
                    var treeProp = nameResolver.PropertyTree.children.Where(x => x.property.Name == prop.Name).First();
                    bool newlyResolved = false;
                    tree = nameResolver.ResolveObject(treeProp, prop.Name, out newlyResolved);
                    if (newlyResolved)
                    {
                        foreach (var child in tree.children)
                        {
                            tree.selectedProperties.Add(child);
                        }
                        BuildJoin(builder, nameResolver, tree, treeProp);
                    }
                    for (int x = 1; x < expansion.Type.Length; x++)
                    {
                        prop = expansion.Member[x];
                        treeProp = tree.children.Where(p => p.property.Name == prop.Name).First();
                        tree = nameResolver.ResolveObject(treeProp, prop.Name, out newlyResolved);
                        if (newlyResolved)
                        {
                            foreach (var child in tree.children)
                            {
                                tree.selectedProperties.Add(child);

                            }
                            BuildJoin(builder, nameResolver, tree, treeProp);
                        }
                    }
                }
            }
            return builder.ToString();
        }
        private void BuildJoin(StringBuilder builder, INameResolver nameResolver, PropertyTree tree, PropertyTreeProperty treeProp) {
            builder.Append(" LEFT JOIN ");
            builder.Append(nameResolver.ResolveObjectAlias(tree));
            builder.Append(" ON (");
                    DataRelationship relationship = treeProp.property.Relationship;
                    if (relationship is OneToManyRelationship) {
                        builder.Append(tree.objectAlias);
                        builder.Append(".");
                        builder.Append(relationship.KeyColumn);
                        builder.Append(" = ");
                        builder.Append(treeProp.root.objectAlias);
                        builder.Append(".");
                        builder.Append(treeProp.root.dbo.PrimaryKey);
                    }
                    else if (relationship is ManyToManyRelationship) {
                         builder.Append(tree.objectAlias);
                        builder.Append(".");
                        builder.Append(relationship.KeyColumn);
                        builder.Append(" = ");
                        builder.Append(treeProp.root.objectAlias);
                        builder.Append(".");
                        builder.Append(treeProp.root.dbo.PrimaryKey);
                    }
                    else if (relationship is ManyToOneRelationship) {
                         builder.Append(tree.objectAlias);
                        builder.Append(".");
                        builder.Append(relationship.KeyColumn);
                        builder.Append(" = ");
                        builder.Append(treeProp.root.objectAlias);
                        builder.Append(".");
                        builder.Append(treeProp.root.dbo.PrimaryKey);
                    }
                    builder.Append(")");
        }

        public string WriteUriFragment(object value, INameResolver namedResolver)
        {
            throw new NotImplementedException();
        }
    }
}
