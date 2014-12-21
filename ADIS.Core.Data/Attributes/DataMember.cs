using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class DataMember : Attribute
    {
        protected string columnName;
        protected bool primaryKey;
        protected int dataLength;
        protected bool nullable;
        public DataMember(string columnName, int dataLength = -1)
        {
            this.primaryKey = false;
            this.columnName = columnName;
            this.dataLength = dataLength;
            this.nullable = false;
        }
        public DataMember(string columnName, bool primaryKey, int dataLength = -1)
        {
            this.columnName = columnName;
            this.primaryKey = primaryKey;
            this.dataLength = dataLength;
            this.nullable = false;
        }
        public DataMember(string columnName, bool primaryKey, bool nullable, int datalength = -1)
        {
            this.columnName = columnName;
            this.primaryKey = primaryKey;
            this.dataLength = dataLength;
            this.nullable = nullable;
        }
        public string ColumnName
        {
            get
            {
                return columnName;
            }
        }
        public bool PrimaryKey
        {
            get
            {
                return primaryKey;
            }
        }
        public bool Nullable {
            get {
                return nullable;
            }
        }
        public int Length {
            get {
                return dataLength;
            }
        }
    }
}
