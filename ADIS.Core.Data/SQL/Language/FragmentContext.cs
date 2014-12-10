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

        public string NextParameter()
        {
            lock (this)
            {
                return ":p" + parameterIndex++;
            }
        }
        public string NextTable()
        {
            lock (this)
            {
                return ":t" + tableIndex++;
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
