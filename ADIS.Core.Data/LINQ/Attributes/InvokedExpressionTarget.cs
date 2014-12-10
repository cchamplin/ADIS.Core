using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.LINQ.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Method | System.AttributeTargets.Property)]
    public class InvokedExpressionTarget : Attribute
    {
        private string _targetSite;
        public InvokedExpressionTarget(String targetSite)
        {
            this._targetSite = targetSite;
        }
        public virtual string TargetSite
        {
            get
            {
                return _targetSite;
            }
        }
    }
}
