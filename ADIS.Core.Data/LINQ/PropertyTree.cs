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
        }
    }
    public class PropertyTreeProperty
    {
        public DataBoundProperty property;
        public string propertyAlias;
        public PropertyTree objectTree;
        public PropertyTree root;
        public PropertyTreeProperty(PropertyTree root, DataBoundProperty property)
        {
            this.property = property;
            this.root = root;
        }
    }
}
