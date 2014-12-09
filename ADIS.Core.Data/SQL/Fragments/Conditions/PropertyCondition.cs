using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.SQL
{
    public class PropertyCondition : Conditional
    {
        protected object left;
        protected bool literalLeft;
        protected bool literalRight;
        protected object right;
        protected string op;
        public PropertyCondition()
        {
            op = "=";
        }
        public object Left
        {
            get
            {
                return left;
            }
        }
        public void SetLeft(object value)
        {
            left = value;
            literalLeft = false;
        }
        public void SetLiteralLeft(string value)
        {
            left = value;
            literalLeft = true;
        }
        public void SetLiteralLeft(string tableAlias, string value)
        {
            left = tableAlias + "." + value;
            literalLeft = true;
        }

        public void SetPropertyLeft(string tableAlias, DataBoundProperty property)
        {
            left = tableAlias + "." + property.ColumnName;
            literalLeft = true;
        }
        public void SetPropertyLeft(string tableAlias, DataProperty property)
        {
            left = tableAlias + "." + ((DataBoundProperty)property).ColumnName;
            literalLeft = true;
        }
        
        public object Right
        {
            get
            {
                return right;
            }
        }
        public void SetRight(object value)
        {
            right = value;
            literalRight = false;
        }
        public void SetLiteralRight(string value)
        {
            right = value;
            literalRight = true;
        }
        public void SetLiteralRight(string tableAlias, string value)
        {
            right = tableAlias + "." + value;
            literalRight = true;
        }
        public void SetPropertyRight(string tableAlias, DataBoundProperty property)
        {
            right = tableAlias + "." + property.ColumnName;
            literalRight = true;
        }
        public void SetPropertyRight(string tableAlias, DataProperty property)
        {
            right = tableAlias + "." + ((DataBoundProperty)property).ColumnName;
            literalRight = true;
        }
        public string Operation
        {
            get
            {
                return op;
            }
            set
            {
                op = value;
            }
        }
        public override string ToSQL(FragmentContext context)
        {
            StringBuilder sb = new StringBuilder();
            int idx;
            if (literalLeft)
            {
                sb.Append(left);
                
            }
            else
            {
                idx = context.NextParameterIndex();
                sb.Append(":parameter");
                sb.Append(idx);
                context.AddParameter(":parameter"+idx,left);
            }
            sb.Append(op);
            if (literalRight)
            {
                sb.Append(right);
            }
            else
            {
                idx = context.NextParameterIndex();
                sb.Append(":parameter");
                sb.Append(idx);
                context.AddParameter(":parameter" + idx, right);
            }
           
            return sb.ToString();
        }
    }
}
