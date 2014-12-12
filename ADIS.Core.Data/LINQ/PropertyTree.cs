using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.LINQ
{
    public class PropertyTree
    {
        public string objectReference;
        
        public List<PropertyTreeProperty> children;
        public List<PropertyTreeProperty> orderedProperties;
        public List<PropertyTreeProperty> selectedProperties;
        public string objectName;
        public bool projected;
        public string objectAlias;
        public IDataBound dbo;
        public PropertyTree(IDataBound dbo, string objectName, string objectReference, string alias)
        {
            children = new List<PropertyTreeProperty>();
            orderedProperties = new List<PropertyTreeProperty>();
            selectedProperties = new List<PropertyTreeProperty>();
            this.objectName = objectName;
            this.objectReference = objectReference;
            this.objectAlias = alias;
            this.dbo = dbo;
            this.projected = false;
        }
    }
    public class PropertyTreeProperty
    {
        public DataBoundProperty property;
        public string propertyAlias;
        public PropertyTree objectTree;
        public PropertyTree root;
        protected int ordinal;
        protected bool columnar;

        public bool Columnar
        {
            get
            {
                return columnar;
            }
        }

        public void SetOrdinal(int ordinal)
        {
            this.ordinal = ordinal;
        }
        public int Ordinal
        {
            get
            {
                return ordinal;
            }
        }
        public PropertyTreeProperty(PropertyTree root, DataBoundProperty property)
        {
            this.property = property;
            this.columnar = false;
            if (!string.IsNullOrWhiteSpace(property.ColumnName))
            {
                columnar = true;
            }
            this.root = root;
        }
        public void BuildJoin(StringBuilder builder, INameResolver nameResolver)
        {
            PropertyTree tree = this.objectTree;
            PropertyTreeProperty treeProp = this;
            DataRelationship relationship = treeProp.property.Relationship;
            if (relationship is OneToManyRelationship)
            {
                builder.Append(" LEFT JOIN ");
                builder.Append(nameResolver.ResolveObjectAlias(tree));
                builder.Append(" ON (");
                builder.Append(tree.objectAlias);
                builder.Append(".");
                builder.Append(relationship.KeyColumn);
                builder.Append(" = ");
                builder.Append(treeProp.root.objectAlias);
                builder.Append(".");
                builder.Append(treeProp.root.dbo.PrimaryKey.ColumnName);
                builder.Append(")");
                builder.AppendLine();
            }
            else if (relationship is ManyToManyRelationship)
            {
                var rel = relationship as ManyToManyRelationship;
                builder.Append(" LEFT JOIN ");
                builder.Append(rel.Schema);
                builder.Append(".");
                builder.Append(rel.Table);
                var crossAlias = nameResolver.NextTableAlias();
                builder.Append(" AS ");
                builder.Append(crossAlias);
                builder.Append(" ON (");
                builder.Append(crossAlias);
                builder.Append(".");
                builder.Append(rel.KeyColumn);
                builder.Append(" = ");
                builder.Append(treeProp.root.objectAlias);
                builder.Append(".");
                builder.Append(treeProp.root.dbo.PrimaryKey.ColumnName);
                builder.Append(") ");
                builder.AppendLine();

                builder.Append(" LEFT JOIN ");
                builder.Append(nameResolver.ResolveObjectAlias(tree));
                builder.Append(" ON (");
                builder.Append(tree.objectAlias);
                builder.Append(".");
                builder.Append(rel.FkColumn);
                builder.Append(" = ");
                builder.Append(crossAlias);
                builder.Append(".");
                builder.Append(tree.dbo.PrimaryKey.ColumnName);
                builder.AppendLine();

            }
            else if (relationship is ManyToOneRelationship)
            {
                builder.Append(" LEFT JOIN ");
                builder.Append(nameResolver.ResolveObjectAlias(tree));
                builder.Append(" ON (");
                builder.Append(tree.objectAlias);
                builder.Append(".");
                builder.Append(tree.dbo.PrimaryKey.ColumnName);
                builder.Append(" = ");
                builder.Append(treeProp.root.objectAlias);
                builder.Append(".");
                builder.Append(relationship.KeyColumn);
                builder.Append(")");
                builder.AppendLine();
            }

        }
    }


}
