using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.SQL
{
    public abstract class FragmentContext
    {
        protected int parameterIndex;
        protected int tableIndex;        
        protected Dictionary<string,object> parameters;
        public FragmentContext()
        {
            parameters = new Dictionary<string, object>();
        }

        public int NextParameterIndex()
        {
            lock (this)
            {
                return parameterIndex++;
            }
        }
        public int NextTableIndex()
        {
            lock (this)
            {
                return tableIndex++;
            }
        }
        public Dictionary<string, object> Parameters
        {
            get
            {
                return parameters;
            }
        }
        public void AddParameter(string name, object value)
        {
            parameters.Add(name, value);
        }
    }
}
