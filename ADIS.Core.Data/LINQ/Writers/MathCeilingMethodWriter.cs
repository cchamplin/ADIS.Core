using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.LINQ.Writers
{
    internal class MathCeilingMethodWriter : MathMethodWriter
    {
        protected override string MethodName
        {
            get { return "ceiling"; }
        }

        public override bool CanHandle(MethodCallExpression expression)
        {
            return expression.Method.DeclaringType == typeof(Math)
                       && expression.Method.Name == "Ceiling";
        }
    }
}
