using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.LINQ.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Method | System.AttributeTargets.Property)]
    public class InvokedExpression : Attribute
    {
    }
}
