using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data.LINQ.Writers.Interfaces;

namespace ADIS.Core.Data.LINQ.Writers
{
    internal class BooleanValueWriter : IValueWriter
    {
        public bool Handles(Type type)
        {
          
                return type == typeof(bool);
        }


        public string WriteSqlFragment(object value)
        {
            var boolean = (bool)value;

            return boolean ? "true" : "false";
        }

        public string WriteUriFragment(object value)
        {
            var boolean = (bool)value;

            return boolean ? "true" : "false";
        }

    }
}
