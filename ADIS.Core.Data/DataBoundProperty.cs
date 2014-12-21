using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data
{
    public class DataBoundProperty : DataProperty
    {
        private string columnName;
        private DataRelationship relationship;
        public DataBoundProperty(Type t, PropertyInfo pi)
            : base(t,pi)
        {
             foreach (Attribute attr in pi.GetCustomAttributes(false)) {
               if (attr is DataMember) {
                   this.columnName = ((DataMember)attr).ColumnName;
               }
               if (attr is OneToMany)
               {
                   this.relationship = new OneToManyRelationship(((OneToMany)attr).KeyColumn);
               }
               if (attr is ManyToMany)
               {
                   this.columnName = ((ManyToMany)attr).KeyColumn;
                   this.relationship = new ManyToManyRelationship(((ManyToMany)attr).Schema, ((ManyToMany)attr).Table, ((ManyToMany)attr).FkColumn,((ManyToMany)attr).KeyColumn);
               }
               if (attr is ManyToOne)
               {
                   this.relationship = new ManyToOneRelationship(((ManyToOne)attr).KeyColumn);
               }
            }
        }
        public string ColumnName
        {
            get
            {
                return columnName;
            }
        }
        public DataRelationship Relationship
        {
            get
            {
                return relationship;
            }
        }
    }
}
