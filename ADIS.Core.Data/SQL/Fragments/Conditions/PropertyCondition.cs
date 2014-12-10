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
        public PropertyCondition(object left, object right, string op = "=")
        {
            this.literalLeft = false;
            this.literalRight = false;
            this.left = left;
            this.right = right;
            this.op = op;
        }
        public PropertyCondition(string tableAlias, string literalLeft, object right, string op = "=")
        {
            this.literalLeft = true;
            this.literalRight = false;
            this.left = tableAlias + "." + literalLeft;
            this.right = right;
            this.op = op;
        }
        public PropertyCondition(string tableAliasLeft, string literalLeft, string tableAliasRight, string literalRight, string op = "=")
        {
            this.literalLeft = true;
            this.literalRight = true;
            this.left = tableAliasLeft + "." + literalLeft;
            this.right = tableAliasRight + "." + literalRight;
            this.op = op;
        }
        public PropertyCondition(string tableAliasLeft, DataBoundProperty propertyLeft, object right, string op = "=")
        {
            this.literalLeft = true;
            this.literalRight = false;
            this.left = tableAliasLeft + "." + propertyLeft.ColumnName;
            this.right = right;
            this.op = op;
        }
        public PropertyCondition(string tableAliasLeft, DataBoundProperty propertyLeft, string tableAliasRight, string literalRight, string op = "=")
        {
            this.literalLeft = true;
            this.literalRight = true;
            this.left = tableAliasLeft + "." + propertyLeft.ColumnName;
            this.right = tableAliasRight + "." + literalRight;
            this.op = op;
        }
        public PropertyCondition(string tableAliasLeft, DataBoundProperty propertyLeft, string tableAliasRight, DataBoundProperty propertyRight, string op = "=")
        {
            this.literalLeft = true;
            this.literalRight = true;
            this.left = tableAliasLeft + "." + propertyLeft.ColumnName;
            this.right = tableAliasRight + "." + propertyRight.ColumnName;
            this.op = op;
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
            string idx;
            if (literalLeft)
            {
                sb.Append(left);
                
            }
            else
            {
                idx = context.NextParameter();
                sb.Append(idx);
                context.AddParameter(idx,left);
            }
            sb.Append(op);
            if (literalRight)
            {
                sb.Append(right);
            }
            else
            {
                idx = context.NextParameter();
                sb.Append(idx);
                context.AddParameter(idx, left);
            }
           
            return sb.ToString();
        }
    }
}
