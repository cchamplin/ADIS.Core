using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Configuration
{
    public class ServiceConfigRecord
    {
        protected string assembly;
        protected string type;
        protected string @interface;
       
        public string Interface
        {
            get { return @interface; }
            set { @interface = value; }
        }
        public string Assembly
        {
            get
            {
                return assembly;
            }
            set
            {
                assembly = value;
            }
        }
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }
    }
}
