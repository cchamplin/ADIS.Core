using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data.LINQ.Attributes;
using ADIS.Core.Data.LINQ.Writers.Interfaces;

namespace ADIS.Core.Data.LINQ.Writers
{
    internal class PassThroughMethodWriter : IMethodCallWriter
    {
        private static readonly MemberInfo[] invokedExpressions = GetInvokedExpressionMethods();

        private static MemberInfo[] GetInvokedExpressionMethods()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes()
             .SelectMany(x => x.GetMembers())
             .Union(assembly.GetTypes())
             .Where(x => Attribute.IsDefined(x, typeof(InvokedExpressionTarget)));
            return types.ToArray();
        }

        public bool CanHandle(MethodCallExpression expression)
        {
            var attributes = System.Attribute.GetCustomAttributes(expression.Method);

            foreach (System.Attribute attr in attributes)
            {
                if (attr is InvokedExpression)
                {
                    return true;
                }
            }
            return false;
        }

        private static object GetValue(Expression input)
        {

            var objectMember = Expression.Convert(input, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember).Compile();

            return getterLambda();
        }



        public string HandleQueryFragment(MethodCallExpression expression, Func<Expression, string> expressionWriter, Type sourceType)
        {
            var methodName = expression.Method.Name;
            var declaringType = expression.Method.DeclaringType;

            var attributes = System.Attribute.GetCustomAttributes(expression.Method);

            foreach (System.Attribute attr in attributes)
            {
                if (attr is InvokedExpression)
                {
                    foreach (MemberInfo member in invokedExpressions)
                    {
                        var attribute = (InvokedExpressionTarget)member.GetCustomAttribute(typeof(InvokedExpressionTarget));
                        if (attribute.TargetSite == declaringType.Name)
                        {
                            if (member.Name == methodName)
                            {
                                Object passThrough = GetValue(Expression.Convert(expression.Arguments[0], expression.Arguments[0].Type));
                                return (String)((MethodInfo)member).Invoke(null, new object[] { passThrough });
                            }
                        }
                    }
                }
            }
            return null;
        }

        public string HandleUriFragment(MethodCallExpression expression, Func<Expression, string> expressionWriter, Type sourceType)
        {
            var methodName = expression.Method.Name;
            var declaringType = expression.Method.DeclaringType;

            var attributes = System.Attribute.GetCustomAttributes(expression.Method);

            foreach (System.Attribute attr in attributes)
            {
                if (attr is InvokedExpression)
                {
                    foreach (MemberInfo member in invokedExpressions)
                    {
                        var attribute = (InvokedExpressionTarget)member.GetCustomAttribute(typeof(InvokedExpressionTarget));
                        if (attribute.TargetSite == declaringType.Name)
                        {
                            if (member.Name == methodName)
                            {
                                Object passThrough = GetValue(Expression.Convert(expression.Arguments[0], expression.Arguments[0].Type));
                                return (String)((MethodInfo)member).Invoke(null, new object[] { passThrough });
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
