using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Validation
{
    public abstract class Validator<T>
    {
        private List<ValidationRule> rules;
        public Validator()
        {
            rules = new List<ValidationRule>();
        }
        public IRuleChain<T,TProperty> Rule<TProperty>(Expression<Func<T,TProperty>> expression)
        {
            MemberInfo member;
            bool checkNulls = false;
            if (expression.Body is UnaryExpression)
            {
                member = (((UnaryExpression)expression.Body).Operand as MemberExpression).Member;
            }
            else
            {
                if (((MemberExpression)expression.Body).Expression.NodeType == ExpressionType.MemberAccess)
                {
                    checkNulls = true;
                }
                member = (expression.Body as MemberExpression).Member;
            }
            ValidationRule rule;
            Func<T, TProperty> compiled;
            if (checkNulls)
            {
                compiled = expression.NullSafeEval();

            }
            else
            {
                compiled = expression.Compile();

                

            }
            rule = ValidationRule.Create(member, x => compiled((T)x), expression);
            rules.Add(rule);
            return new RuleChain<T,TProperty>(rule);
        }
        public virtual ValidationResult Validate(T instance)
        {
            return new ValidationResult(rules.SelectMany(x => x.Validate(instance)).ToList());
        }
       
    }
}
