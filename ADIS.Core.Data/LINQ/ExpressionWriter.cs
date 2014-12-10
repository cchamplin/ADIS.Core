using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data.LINQ.Attributes;
using ADIS.Core.Data.LINQ.Writers;
using ADIS.Core.Data.LINQ.Writers.Interfaces;

namespace ADIS.Core.Data.LINQ
{
    internal class ExpressionWriter : IExpressionWriter
    {
        
        private static readonly ExpressionType[] CompositeExpressionTypes = new[] { ExpressionType.Or, ExpressionType.OrElse, ExpressionType.And, ExpressionType.AndAlso };
        private static readonly MemberInfo[] invokedExpressions = GetInvokedExpressionMethods();
        private readonly IExpressionValueWriter[] _expressionValueWriters = new IExpressionValueWriter[] {
                                                                   new MemberAccessWriter(),
        };
        private readonly IMethodCallWriter[] _methodCallWriters = new IMethodCallWriter[]
																{
																	new EqualsMethodWriter(), 
																	new StringReplaceMethodWriter(), 
																	new StringTrimMethodWriter(), 
																	new StringToLowerMethodWriter(), 
																	new StringToUpperMethodWriter(), 
																	new StringSubstringMethodWriter(), 
																	new StringContainsMethodWriter(), 
																	new StringIndexOfMethodWriter(), 
																	new StringEndsWithMethodWriter(), 
																	new StringStartsWithMethodWriter(), 
																	new MathRoundMethodWriter(), 
																	new MathFloorMethodWriter(), 
																	new MathCeilingMethodWriter(),
 																	new EmptyAnyMethodWriter(), 
																	new AnyAllMethodWriter(),
                                                                    new PassThroughMethodWriter(),
																	new DefaultMethodWriter()
																};



        FragmentWriterType writerType;
        private readonly INameResolver _memberNameResolver;
        public ExpressionWriter(INameResolver memberNameResolver, FragmentWriterType writerType)
        {
            this._memberNameResolver = memberNameResolver;
            this.writerType = writerType;
        }
        private static MemberInfo[] GetInvokedExpressionMethods()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes()
             .SelectMany(x => x.GetMembers())
             .Union(assembly.GetTypes())
             .Where(x => Attribute.IsDefined(x, typeof(InvokedExpressionTarget)));
            return types.ToArray();
        }
        public string Write(Expression expression, Type sourceType)
        {
            return expression == null ? null : Write(expression, expression.Type, GetRootParameterName(expression), sourceType);
        }

        private static Type GetUnconvertedType(Expression expression)
        {


            switch (expression.NodeType)
            {
                case ExpressionType.Convert:
                    var unaryExpression = expression as UnaryExpression;



                    return unaryExpression.Operand.Type;
                default:
                    return expression.Type;
            }
        }

       

       

        private static object GetValue(Expression input)
        {


            var objectMember = Expression.Convert(input, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember).Compile();

            return getterLambda();
        }

        

        private static string GetOperation(Expression expression)
        {


            switch (expression.NodeType)
            {
                case ExpressionType.Add:
                    return "add";
                case ExpressionType.AddChecked:
                    break;
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return "and";
                case ExpressionType.Divide:
                    return "div";
                case ExpressionType.Equal:
                    return "eq";
                case ExpressionType.GreaterThan:
                    return "gt";
                case ExpressionType.GreaterThanOrEqual:
                    return "ge";
                case ExpressionType.LessThan:
                    return "lt";
                case ExpressionType.LessThanOrEqual:
                    return "le";
                case ExpressionType.Modulo:
                    return "mod";
                case ExpressionType.Multiply:
                    return "mul";
                case ExpressionType.Not:
                    return "not";
                case ExpressionType.NotEqual:
                    return "ne";
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return "or";
                case ExpressionType.Subtract:
                    return "sub";
            }

            return string.Empty;
        }

        private ParameterExpression GetRootParameterName(Expression expression)
        {
            /*
            if (expression is UnaryExpression)
            {
                expression = ((UnaryExpression)expression).Operand;
            }

            if (expression is LambdaExpression && ((LambdaExpression)expression).Parameters.Count() > 0)
            {
                if (((LambdaExpression)expression).Parameters.First().Type.IsGenericType)
                {
                    if (((LambdaExpression)expression).Body is BinaryExpression)
                    {
                        var body = (BinaryExpression)((LambdaExpression)expression).Body;
                        var paramExpression = ResolveParameterExpression(body.Left) as RootedExpression;
                        if (paramExpression != null)
                        {
                            if (paramExpression.Parameter.Name == ((LambdaExpression)expression).Parameters.First().Name)
                            {
                                return paramExpression.Member.Type.Name;
                            }
                        }
                        
                    }
                }
                return ((LambdaExpression)expression).Parameters.First().Name;
            }
            if (expression is MemberExpression)
           {
               return ((MemberExpression)expression).Member.DeclaringType.Name;
            }

            return null;
             * */

            if (expression is UnaryExpression)
            {
                expression = ((UnaryExpression)expression).Operand;
            }

            if (expression is LambdaExpression && ((LambdaExpression)expression).Parameters.Any())
            {
                var paramName = ((LambdaExpression)expression).Parameters.First();
                _memberNameResolver.SetRoot(paramName.Type,paramName.Name);
            }

            return null;
        }
        private class RootedExpression : Expression
        {
            public MemberExpression Member;
            public ParameterExpression Parameter;
        }
        private static Expression ResolveParameterExpression(Expression expression)
        {
            while (expression is MemberExpression)
            {
                expression = ((MemberExpression)expression).Expression;
                if (((MemberExpression)expression).Expression is ParameterExpression)
                    return new RootedExpression { Member = (MemberExpression)expression, Parameter = ((MemberExpression)expression).Expression as ParameterExpression };

            }
            return expression;
        }
        private string Write(Expression expression, ParameterExpression rootParameter, Type sourceType)
        {
            return expression == null ? null : Write(expression, expression.Type, rootParameter, sourceType);
        }

        private string Write(Expression expression, Type type, ParameterExpression rootParameter, Type sourceType)
        {

            switch (expression.NodeType)
            {
                case ExpressionType.Parameter:
                    var parameterExpression = expression as ParameterExpression;



                    return parameterExpression.Name;
                case ExpressionType.Constant:
                    {
                        var value = GetValue(Expression.Convert(expression, type));
                        return ParameterValueWriter.Write(value,_memberNameResolver, writerType);
                    }

                case ExpressionType.Add:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Divide:
                case ExpressionType.Equal:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.Modulo:
                case ExpressionType.Multiply:
                case ExpressionType.NotEqual:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.Subtract:
                    return WriteBinaryExpression(expression, rootParameter, sourceType);
                /*
                {
                    var binaryExpression = expression as BinaryExpression;



                    var operation = GetOperation(binaryExpression);

                    if (binaryExpression.Left.NodeType == ExpressionType.Call)
                    {
                        var compareResult = ResolveCompareToOperation(rootParameterName, (MethodCallExpression)binaryExpression.Left, operation, binaryExpression.Right as ConstantExpression, resolutions);
                        if (compareResult != null)
                        {
                            return compareResult;
                        }
                    }

                    if (binaryExpression.Right.NodeType == ExpressionType.Call)
                    {
                        var compareResult = ResolveCompareToOperation(rootParameterName, (MethodCallExpression)binaryExpression.Right, operation, binaryExpression.Left as ConstantExpression, resolutions);
                        if (compareResult != null)
                        {
                            return compareResult;
                        }
                    }

                    var isLeftComposite = CompositeExpressionTypes.Any(x => x == binaryExpression.Left.NodeType);
                    var isRightComposite = CompositeExpressionTypes.Any(x => x == binaryExpression.Right.NodeType);

                    var leftType = GetUnconvertedType(binaryExpression.Left);
                    var leftString = Write(binaryExpression.Left, rootParameterName, resolutions);
                    var rightString = Write(binaryExpression.Right, leftType, rootParameterName, resolutions);

                    return string.Format(
                                         "{0} {1} {2}",
                                         string.Format(isLeftComposite ? "({0})" : "{0}", leftString),
                                         operation,
                                         string.Format(isRightComposite ? "({0})" : "{0}", rightString));
                     
                }*/

                case ExpressionType.Negate:
                    return WriteNegate(expression, rootParameter, sourceType);
                /* {
                     var unaryExpression = expression as UnaryExpression;



                     var operand = unaryExpression.Operand;

                     return string.Format("-{0}", Write(operand, rootParameterName, resolutions));
                 }*/

                case ExpressionType.Not:
                case ExpressionType.IsFalse:
                    return WriteFalse(expression, rootParameter, sourceType);
                /*{
                    var unaryExpression = expression as UnaryExpression;


                    var operand = unaryExpression.Operand;

                    return string.Format("not({0})", Write(operand, rootParameterName, resolutions));
                }*/

                case ExpressionType.IsTrue:
                    return WriteTrue(expression, rootParameter, sourceType);
                /*{
                    var unaryExpression = expression as UnaryExpression;

                    //Contract.Assume(unaryExpression != null);

                    var operand = unaryExpression.Operand;

                    return Write(operand, rootParameterName, resolutions);
                }*/

                case ExpressionType.Convert:
                case ExpressionType.Quote:
                    return WriteConversion(expression, rootParameter, sourceType);
                /*
                {
                    var unaryExpression = expression as UnaryExpression;



                    var operand = unaryExpression.Operand;
                    return Write(operand, rootParameterName, resolutions);
                }*/

                case ExpressionType.MemberAccess:
                    return WriteMemberAccess(expression, rootParameter, sourceType);
                /*
                {
                    var memberExpression = expression as MemberExpression;


                    if (memberExpression.Expression == null)
                    {
                        var memberValue = GetValue(memberExpression);
                        return ParameterValueWriter.Write(memberValue);
                    }

                    var pathPrefixes = new List<string>();

                    var currentMemberExpression = memberExpression;


                    while (currentMemberExpression != null)
                    {
                        if (resolutions.ContainsKey(currentMemberExpression.Member.Name))
                        {
                            if (resolutions[currentMemberExpression.Member.Name] != null)
                            {
                                pathPrefixes.Add(resolutions[currentMemberExpression.Member.Name]);
                            }
                            break;
                        }
                        else
                            pathPrefixes.Add(currentMemberExpression.Member.Name);
                        if (currentMemberExpression.Expression is ParameterExpression && ((ParameterExpression)currentMemberExpression.Expression).Name != rootParameterName)
                        {
                            pathPrefixes.Add(((ParameterExpression)currentMemberExpression.Expression).Name);
                        }

                        currentMemberExpression = currentMemberExpression.Expression as MemberExpression;
                    }


                    pathPrefixes.Reverse();
                    var prefix = string.Join("!2F", pathPrefixes);

                    if (!IsMemberOfParameter(memberExpression))
                    {
                        var collapsedExpression = CollapseCapturedOuterVariables(memberExpression);
                        if (!(collapsedExpression is MemberExpression))
                        {


                            return Write(collapsedExpression, rootParameterName, resolutions);
                        }

                        memberExpression = (MemberExpression)collapsedExpression;
                    }

                    var memberCall = GetMemberCall(memberExpression);

                    var innerExpression = memberExpression.Expression;



                    return string.IsNullOrWhiteSpace(memberCall)
                            ? prefix
                            : string.Format("{0}({1})", memberCall, Write(innerExpression, rootParameterName, resolutions));
                }
                */
                case ExpressionType.Call:
                    return WriteCall(expression, rootParameter, sourceType);
                /*
                var methodCallExpression = expression as MethodCallExpression;



                return GetMethodCall(methodCallExpression, rootParameterName, resolutions);
                 * */
                case ExpressionType.New:
                case ExpressionType.ArrayIndex:
                case ExpressionType.ArrayLength:
                case ExpressionType.Conditional:
                case ExpressionType.Coalesce:
                    var newValue = GetValue(expression);
                    return ParameterValueWriter.Write(newValue,_memberNameResolver, writerType);
                case ExpressionType.Lambda:
                    return WriteLambda(expression, rootParameter, sourceType);
                /*
                var lambdaExpression = expression as LambdaExpression;



                var body = lambdaExpression.Body;
                return Write(body, rootParameterName, resolutions);
                 */
                default:
                    throw new InvalidOperationException("Expression is not recognized or supported");
            }
        }


        private string WriteLambda(Expression expression, ParameterExpression rootParameter, Type sourceType)
        {
            var lambdaExpression = expression as LambdaExpression;

            var body = lambdaExpression.Body;
            return Write(body, rootParameter, sourceType);
        }

        private string WriteFalse(Expression expression, ParameterExpression rootParameter, Type sourceType)
        {
            var unaryExpression = expression as UnaryExpression;

            var operand = unaryExpression.Operand;

            return string.Format("not({0})", Write(operand, rootParameter, sourceType));
        }

        private string WriteTrue(Expression expression, ParameterExpression rootParameter, Type sourceType)
        {
            var unaryExpression = expression as UnaryExpression;

            var operand = unaryExpression.Operand;

            return Write(operand, rootParameter, sourceType);
        }

        private string WriteConversion(Expression expression, ParameterExpression rootParameter, Type sourceType)
        {
            var unaryExpression = expression as UnaryExpression;

            var operand = unaryExpression.Operand;
            return Write(operand, rootParameter, sourceType);
        }

        private string WriteCall(Expression expression, ParameterExpression rootParameter, Type sourceType)
        {
            var methodCallExpression = expression as MethodCallExpression;

            return GetMethodCall(methodCallExpression, rootParameter, sourceType);
        }

        private string WriteMemberAccess(Expression expression, ParameterExpression rootParameter, Type sourceType)
        {
            var expressionWriter = _expressionValueWriters.FirstOrDefault(w => w.CanHandle(expression));
            if (expressionWriter == null)
            {
                throw new NotSupportedException(expression + " is not supported");
            }
            switch (writerType)
            {
                case FragmentWriterType.SQL:
                    return expressionWriter.HandleQueryFragment(expression, rootParameter,_memberNameResolver, e => Write(e, rootParameter, sourceType), sourceType);
                case FragmentWriterType.URI:
                    return expressionWriter.HandleUriFragment(expression, rootParameter, _memberNameResolver, e => Write(e, rootParameter, sourceType), sourceType);
            }
            throw new NotSupportedException("Fragment writer type is not supported");
        }

        private string WriteNegate(Expression expression, ParameterExpression rootParameter, Type sourceType)
        {
            var unaryExpression = expression as UnaryExpression;

            var operand = unaryExpression.Operand;

            return string.Format("-{0}", Write(operand, rootParameter, sourceType));
        }

        private string WriteBinaryExpression(Expression expression, ParameterExpression rootParameter, Type sourceType)
        {
            var binaryExpression = expression as BinaryExpression;

            var operation = GetOperation(binaryExpression);

            if (binaryExpression.Left.NodeType == ExpressionType.Call)
            {
                var compareResult = ResolveCompareToOperation(rootParameter,
                                                              (MethodCallExpression)binaryExpression.Left,
                                                              operation,
                                                              binaryExpression.Right as ConstantExpression, sourceType);
                if (compareResult != null)
                {
                    return compareResult;
                }
            }

            if (binaryExpression.Right.NodeType == ExpressionType.Call)
            {
                var compareResult = ResolveCompareToOperation(rootParameter,
                                                              (MethodCallExpression)binaryExpression.Right,
                                                              operation,
                                                              binaryExpression.Left as ConstantExpression, sourceType);
                if (compareResult != null)
                {
                    return compareResult;
                }
            }

            var isLeftComposite = CompositeExpressionTypes.Any(x => x == binaryExpression.Left.NodeType);
            var isRightComposite = CompositeExpressionTypes.Any(x => x == binaryExpression.Right.NodeType);

            var leftType = GetUnconvertedType(binaryExpression.Left);
            var leftString = Write(binaryExpression.Left, rootParameter, sourceType);
            var rightString = Write(binaryExpression.Right, leftType, rootParameter, sourceType);

            return string.Format(
                "{0} {1} {2}",
                string.Format(isLeftComposite
                                  ? "({0})"
                                  : "{0}",
                              leftString),
                operation,
                string.Format(isRightComposite
                                  ? "({0})"
                                  : "{0}",
                              rightString));
        }



        private string ResolveCompareToOperation(
            ParameterExpression rootParameter,
            MethodCallExpression methodCallExpression,
            string operation,
            ConstantExpression comparisonExpression, Type sourceType)
        {
            if (methodCallExpression != null
                && methodCallExpression.Method.Name == "CompareTo"
                && methodCallExpression.Method.ReturnType == typeof(int)
                && comparisonExpression != null
                && Equals(comparisonExpression.Value, 0))
            {
                return string.Format(
                    "{0} {1} {2}",
                    Write(methodCallExpression.Object, rootParameter, sourceType),
                    operation,
                    Write(methodCallExpression.Arguments[0], rootParameter, sourceType));
            }

            return null;
        }


        private string GetMethodCall(MethodCallExpression expression, ParameterExpression rootParameter, Type sourceType)
        {
            var methodCallWriter = _methodCallWriters.FirstOrDefault(w => w.CanHandle(expression));
            if (methodCallWriter == null)
            {
                throw new NotSupportedException(expression + " is not supported");
            }
            switch (writerType)
            {
                case FragmentWriterType.SQL:
                    return methodCallWriter.HandleQueryFragment(expression, e => Write(e, rootParameter, sourceType), sourceType);
                case FragmentWriterType.URI:
                    return methodCallWriter.HandleUriFragment(expression, e => Write(e, rootParameter, sourceType), sourceType);
                default:
                    throw new NotSupportedException("Fragment writer type is not supported");
            }
        }
    }
}
