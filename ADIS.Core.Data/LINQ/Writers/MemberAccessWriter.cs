using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data.LINQ.Writers.Interfaces;

namespace ADIS.Core.Data.LINQ.Writers
{
    internal class MemberAccessWriter : IExpressionValueWriter
    {
        public bool CanHandle(Expression expression)
        {
            return (expression.NodeType == ExpressionType.MemberAccess);
        }

        private bool IsMemberOfParameter(MemberExpression input)
        {
            if (input == null || input.Expression == null)
            {
                return false;
            }

            var nodeType = input.Expression.NodeType;
            var tempExpression = input.Expression as MemberExpression;
            while (nodeType == ExpressionType.MemberAccess)
            {
                if (tempExpression == null || tempExpression.Expression == null)
                {
                    return false;
                }

                nodeType = tempExpression.Expression.NodeType;
                tempExpression = tempExpression.Expression as MemberExpression;
            }

            return nodeType == ExpressionType.Parameter;
        }
        private object GetValue(Expression input)
        {

            var objectMember = Expression.Convert(input, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember).Compile();

            return getterLambda();
        }

        private Expression CollapseCapturedOuterVariables(MemberExpression input)
        {
            if (input == null || input.NodeType != ExpressionType.MemberAccess)
            {
                return input;
            }

            switch (input.Expression.NodeType)
            {
                case ExpressionType.New:
                case ExpressionType.MemberAccess:
                    var value = GetValue(input);
                    return Expression.Constant(value);
                case ExpressionType.Constant:
                    var obj = ((ConstantExpression)input.Expression).Value;
                    if (obj == null)
                    {
                        return input;
                    }

                    var fieldInfo = input.Member as FieldInfo;
                    if (fieldInfo != null)
                    {
                        var result = fieldInfo.GetValue(obj);
                        return result is Expression ? (Expression)result : Expression.Constant(result);
                    }

                    var propertyInfo = input.Member as PropertyInfo;
                    if (propertyInfo != null)
                    {
                        var result = propertyInfo.GetValue(obj, null);
                        return result is Expression ? (Expression)result : Expression.Constant(result);
                    }

                    break;
                case ExpressionType.TypeAs:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    return Expression.Constant(GetValue(input));
            }

            return input;
        }
        private string GetMemberCall(MemberExpression memberExpression)
        {


            var declaringType = memberExpression.Member.DeclaringType;
            var name = memberExpression.Member.Name;

            if (declaringType == typeof(string) && string.Equals(name, "Length"))
            {
                return name.ToLowerInvariant();
            }
            if (declaringType == typeof(DateTime))
            {
                switch (name)
                {
                    case "Hour":
                    case "Minute":
                    case "Second":
                    case "Day":
                    case "Month":
                    case "Year":
                        return name.ToLowerInvariant();
                }
            }

            return string.Empty;
        }

        private class MemberRecord
        {
            public Type sourceType;
            public MemberInfo memberInfo;
        }

        public string HandleQueryFragment(Expression expression, ParameterExpression rootParameterName, INameResolver memberNameResolver, Func<Expression, string> expressionWriter, Type sourceType)
        {
            var memberExpression = expression as MemberExpression;

            if (memberExpression.Expression == null)
            {
                var memberValue = GetValue(memberExpression);
                return ParameterValueWriter.Write(memberValue, memberNameResolver, FragmentWriterType.SQL);
            }

            var pathPrefixes = new List<MemberRecord>();
            var objectPrefixes = new List<string>();
            var currentMemberExpression = memberExpression;
            while (true)
            {
                pathPrefixes.Add(new MemberRecord() { memberInfo = currentMemberExpression.Member, sourceType = currentMemberExpression.Member.DeclaringType });
                if (currentMemberExpression.Expression is ParameterExpression)
                {
                    objectPrefixes.Add(currentMemberExpression.Member.Name);
                    objectPrefixes.Add(((ParameterExpression)currentMemberExpression.Expression).Name);
                }
                else
                {
                    objectPrefixes.Add(currentMemberExpression.Member.Name);
                }
                //if (currentMemberExpression.Expression is PropertyExpression)
                //{
                //    objectPrefixes.Add(((ParameterExpression)currentMemberExpression.Expression).Name);
                //}
               
                var currentMember = currentMemberExpression.Expression as MemberExpression;
                if (currentMember == null)
                {
                    break;
                }

                currentMemberExpression = currentMember;
            }
            objectPrefixes.Reverse();
            pathPrefixes.Reverse();
            var prefix = string.Join("/", pathPrefixes.Select(x => memberNameResolver.GetColumnFromName(x.memberInfo, x.sourceType)).Select(x => x.Item2));
            if (rootParameterName != null
                && currentMemberExpression.Expression is ParameterExpression
                && ((ParameterExpression)currentMemberExpression.Expression).Name != rootParameterName.Name)
            {
                prefix = string.Format("{0}/{1}", ((ParameterExpression)currentMemberExpression.Expression).Name, prefix);
            }

            var member = memberNameResolver.ResolveDataProperty(objectPrefixes);
            prefix = memberNameResolver.ResolveColumn(member);

            if (!IsMemberOfParameter(memberExpression))
            {
                var collapsedExpression = CollapseCapturedOuterVariables(memberExpression);
                if (!(collapsedExpression is MemberExpression))
                {

                    return expressionWriter(collapsedExpression);
                }

                memberExpression = (MemberExpression)collapsedExpression;
            }

            var memberCall = GetMemberCall(memberExpression);

            var innerExpression = memberExpression.Expression;


            return string.IsNullOrWhiteSpace(memberCall)
                       ? prefix
                       : string.Format("{0}({1})", memberCall, expressionWriter(innerExpression));
        }

        public string HandleUriFragment(Expression expression, ParameterExpression rootParameterName, INameResolver memberNameResolver, Func<Expression, string> expressionWriter, Type sourceType)
        {
            var memberExpression = expression as MemberExpression;


            if (memberExpression.Expression == null)
            {
                var memberValue = GetValue(memberExpression);
                return ParameterValueWriter.Write(memberValue,memberNameResolver,FragmentWriterType.URI);
            }

            var pathPrefixes = new List<MemberInfo>();

            var currentMemberExpression = memberExpression;
            while (true)
            {
                pathPrefixes.Add(currentMemberExpression.Member);

                var currentMember = currentMemberExpression.Expression as MemberExpression;
                if (currentMember == null)
                {
                    break;
                }

                currentMemberExpression = currentMember;
            }

            pathPrefixes.Reverse();
            var prefix = string.Join("/", pathPrefixes.Select(x => memberNameResolver.GetNameFromAlias(x, sourceType)).Select(x => x.Item2));
            if (rootParameterName != null
                && currentMemberExpression.Expression is ParameterExpression
                && ((ParameterExpression)currentMemberExpression.Expression).Name != rootParameterName.Name)
            {
                prefix = string.Format("{0}/{1}", ((ParameterExpression)currentMemberExpression.Expression).Name, prefix);
            }

            if (!IsMemberOfParameter(memberExpression))
            {
                var collapsedExpression = CollapseCapturedOuterVariables(memberExpression);
                if (!(collapsedExpression is MemberExpression))
                {
                    return expressionWriter(collapsedExpression);
                }

                memberExpression = (MemberExpression)collapsedExpression;
            }

            var memberCall = GetMemberCall(memberExpression);

            var innerExpression = memberExpression.Expression;


            return string.IsNullOrWhiteSpace(memberCall)
                       ? prefix
                       : string.Format("{0}({1})", memberCall, expressionWriter(innerExpression));
        }
    }
}
