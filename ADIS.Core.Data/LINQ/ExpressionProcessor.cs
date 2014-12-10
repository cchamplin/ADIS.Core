using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.LINQ
{
    internal class ExpressionProcessor : IExpressionProcessor
    {
        private readonly IExpressionWriter _writer;
        public Type rootType;
        private readonly INameResolver _memberNameResolver;
        public ExpressionProcessor(IExpressionWriter writer, INameResolver memberNameResolver)
        {
            //Contract.Requires(writer != null);
            _memberNameResolver = memberNameResolver;
            _writer = writer;
        }

        public object ProcessMethodCall<T>(MethodCallExpression methodCall, QueryBuilder builder, Func<QueryBuilder, bool, IEnumerable<T>> resultLoader, Func<Type, QueryBuilder, IEnumerable> intermediateResultLoader)
        {
            if (methodCall == null)
            {
                return null;
            }

            var method = methodCall.Method.Name;

            switch (method)
            {
                case "First":
                case "FirstOrDefault":
                    builder.TakeParameter = "1";
                    return methodCall.Arguments.Count >= 2
                                ? GetMethodResult(methodCall, builder, resultLoader, intermediateResultLoader)
                                : GetResult(methodCall, builder, resultLoader, intermediateResultLoader);

                case "Single":
                case "SingleOrDefault":
                case "Last":
                case "LastOrDefault":
                case "Count":
                case "LongCount":
                    return methodCall.Arguments.Count >= 2
                            ? GetMethodResult(methodCall, builder, resultLoader, intermediateResultLoader)
                            : GetResult(methodCall, builder, resultLoader, intermediateResultLoader);
                case "SelectMany":
                    {

                        var result = ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);
                        var type = methodCall.Type;

                        var sourceExpr = methodCall.Arguments[0];
                        var mappedRoot = false;
                        if (sourceExpr.Type.IsGenericType)
                        {
                            var source = sourceExpr.Type.GenericTypeArguments[0];
                            if (rootType == null)
                            {
                                rootType = source;
                           //     resolveMapping(methodCall.Arguments[1], methodCall.Arguments[2], true);
                                //ResolveProjection(builder, GetLambda(methodCall.Arguments[1]));
                            }
                        }

                        //resolveMapping(methodCall.Arguments[1], methodCall.Arguments[2]);

                        //var selectionSelector = (LambdaExpression)GetLambda(methodCall.Arguments[1]);
                        //var selectionParameter = _writer.Write(selectionSelector.Body, quickResolution);
                        //builder.SelectParameter = string.IsNullOrWhiteSpace(builder.SelectParameter)
                        //                     ? selectionParameter
                        //                    : builder.SelectParameter + "," + selectionParameter;
                        //
                        //var resultSelector = methodCall.Arguments[2];
                        break;
                    }
                case "Where":
                    //Contract.Assume(methodCall.Arguments.Count >= 2);
                    {
                        var result = ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);
                        if (result != null)
                        {
                            return InvokeEager(methodCall, result);
                        }

                        var newFilter = _writer.Write(methodCall.Arguments[1], builder.SourceType);

                        builder.WhereParameter = string.IsNullOrWhiteSpace(builder.WhereParameter)
                                                    ? newFilter
                                                    : string.Format("({0}) and ({1})", builder.WhereParameter, newFilter);
                    }

                    break;
                case "Select":
                    //Contract.Assume(methodCall.Arguments.Count >= 2);
                    {
                        var result = ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);
                        if (result != null)
                        {
                            return InvokeEager(methodCall, result);
                        }

                        //TESTING FUNCTIONALITY
                        if (!string.IsNullOrWhiteSpace(builder.SelectParameter))
                        {
                            //return null;
                            return ExecuteMethod(methodCall, builder, resultLoader, intermediateResultLoader);
                        }
                        var unaryExpression = methodCall.Arguments[1] as UnaryExpression;
                        if (unaryExpression != null)
                        {
                            var lambdaExpression = unaryExpression.Operand as LambdaExpression;
                            if (lambdaExpression != null)
                            {
                                //if (!ResolveProjection(builder, lambdaExpression))
                                //{
                                   // return ExecuteMethod(methodCall, builder, resultLoader, intermediateResultLoader);
                               // }
                                //else
                               //     return null;
                                var sourceType = builder.SourceType;
                                return ResolveProjection(builder, lambdaExpression, sourceType);
                            }
                        }
                        /*
                        var unaryExpression = methodCall.Arguments[1] as UnaryExpression;
                        if (unaryExpression != null)
                        {
                            var lambdaExpression = unaryExpression.Operand as LambdaExpression;
                            if (lambdaExpression != null)
                            {
                                var selectFunction = lambdaExpression.Body as NewExpression;

                                if (selectFunction != null)
                                {
                                    var members = selectFunction.Members.Select(x => x.Name).ToArray();
                                    var args = selectFunction.Arguments.OfType<MemberExpression>().Select(x => x.Member.Name).ToArray();
                                    if (members.Intersect(args).Count() != members.Length)
                                    {
                                        throw new InvalidOperationException("Projection into new member names is not supported.");
                                    }

                                    builder.SelectParameter = string.Join(",", args);
                                }

                                var propertyExpression = lambdaExpression.Body as MemberExpression;
                                if (propertyExpression != null)
                                {
                                    if (_models.ContainsKey(((PropertyInfo)propertyExpression.Member).PropertyType))
                                    {
                                        string dataSet = propertyExpression.Member.Name;
                                        if (quickResolution.ContainsKey(dataSet))
                                        {
                                            if (quickResolution[dataSet] != null)
                                                dataSet = rootType.Name + "!2F" + quickResolution[dataSet];
                                            else
                                                dataSet = "";
                                        }
                                        else
                                        {
                                            if (rootType == null)
                                                rootType = propertyExpression.Expression.Type;
                                            dataSet = rootType.Name + "!2F" + dataSet;
                                        }
                                        builder.SelectParameter = string.IsNullOrWhiteSpace(builder.SelectParameter)
                                            ? dataSet
                                            : builder.SelectParameter + "," + dataSet;
                                    }
                                    else
                                    {
                                        return ExecuteMethod(methodCall, builder, resultLoader, intermediateResultLoader);
                                        //GetResult(methodCall, builder, resultLoader, intermediateResultLoader);
                                        //The returned member property is not something the webservice
                                        //handles, we need to pull the results
                                        //then compose a list of that type
                                    }
                                }
                            }
                        }*/
                    }

                    break;
                case "OrderBy":
                case "ThenBy":
                    //Contract.Assume(methodCall.Arguments.Count >= 2);
                    {
                        var methodCallExpression = methodCall.Arguments[0] as MethodCallExpression;
                        var result = ProcessMethodCall(methodCallExpression, builder, resultLoader, intermediateResultLoader);
                        if (result != null)
                        {
                            return InvokeEager(methodCall, result);
                        }

                        //var item = _writer.Write(methodCall.Arguments[1], builder.SourceType);
                       //builder.OrderByParameter.Add(item);
                        var sourceType = builder.SourceType;
                        var sortProperty = methodCall.Arguments[1];
                        var item = _writer.Write(sortProperty,sourceType);
                        builder.OrderByParameter.Add(item);
                    }

                    break;
                case "OrderByDescending":
                case "ThenByDescending":
                    //Contract.Assume(methodCall.Arguments.Count >= 2);
                    {
                        var result = ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);
                        if (result != null)
                        {
                            return InvokeEager(methodCall, result);
                        }

                        var visit = _writer.Write(methodCall.Arguments[1], builder.SourceType);
                        builder.OrderByParameter.Add(visit + " desc");
                    }

                    break;
                case "Take":
                    //Contract.Assume(methodCall.Arguments.Count >= 2);
                    {
                        var result = ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);
                        if (result != null)
                        {
                            return InvokeEager(methodCall, result);
                        }

                        builder.TakeParameter = _writer.Write(methodCall.Arguments[1], builder.SourceType);
                    }

                    break;
                case "Skip":
                    //Contract.Assume(methodCall.Arguments.Count >= 2);
                    {
                        var result = ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);
                        if (result != null)
                        {
                            return InvokeEager(methodCall, result);
                        }

                        builder.SkipParameter = _writer.Write(methodCall.Arguments[1], builder.SourceType);
                    }

                    break;
                case "Include":
                case "Expand":
                    //Contract.Assume(methodCall.Arguments.Count >= 2);
                    {
                        var result = ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);
                        if (result != null)
                        {
                            return InvokeEager(methodCall, result);
                        }

                        var expression = methodCall.Arguments[1];
                        builder.ExpandParameter = _writer.Write(methodCall.Arguments[1], builder.SourceType);
                        //Contract.Assume(expression != null);

                       // var objectMember = Expression.Convert(expression, typeof(object));
                       // var getterLambda = Expression.Lambda<Func<object>>(objectMember).Compile();

                       // builder.ExpandParameter = getterLambda().ToString();
                    }

                    break;
                default:
                    return ExecuteMethod(methodCall, builder, resultLoader, intermediateResultLoader);
            }

            return null;
        }


        /*private bool ResolveProjection(QueryBuilder builder, LambdaExpression lambdaExpression)
        {
            var selectFunction = lambdaExpression.Body as NewExpression;

            if (selectFunction != null)
            {
                var members = selectFunction.Members.Select(x => x.Name)
                                            .ToArray();
                var args = selectFunction.Arguments.OfType<MemberExpression>()
                                         .Select(x => x.Member.Name)
                                         .ToArray();
                if (members.Intersect(args).Count() != members.Length)
                {
                    throw new InvalidOperationException("Projection into new member names is not supported.");
                }

                builder.SelectParameter = string.Join(",", args);
            }



            var propertyExpression = lambdaExpression.Body as MemberExpression;
            if (propertyExpression != null)
            {
                if (_models.ContainsKey(((PropertyInfo)propertyExpression.Member).PropertyType))
                {
                    string dataSet = propertyExpression.Member.Name;
                    if (quickResolution.ContainsKey(dataSet))
                    {
                        if (quickResolution[dataSet] != null)
                            dataSet = rootType.Name + "/" + quickResolution[dataSet];
                        else
                            dataSet = "";
                    }
                    else
                    {
                        if (rootType == null)
                            rootType = propertyExpression.Expression.Type;
                        dataSet = rootType.Name + "/" + dataSet;
                    }
                    builder.SelectParameter = string.IsNullOrWhiteSpace(builder.SelectParameter)
                        ? dataSet
                        : builder.SelectParameter + "," + dataSet;
                }
                else if (((PropertyInfo)propertyExpression.Member).PropertyType.IsGenericType && (_models.ContainsKey(((PropertyInfo)propertyExpression.Member).PropertyType.GenericTypeArguments[0])))
                {
                    string dataSet = propertyExpression.Member.Name;
                    if (quickResolution.ContainsKey(dataSet))
                    {
                        if (quickResolution[dataSet] != null)
                            dataSet = rootType.Name + "/" + quickResolution[dataSet];
                        else
                            dataSet = "";
                    }
                    else
                    {
                        if (rootType == null)
                            rootType = propertyExpression.Expression.Type;
                        dataSet = rootType.Name + "/" + dataSet;
                    }
                    builder.SelectParameter = string.IsNullOrWhiteSpace(builder.SelectParameter)
                        ? dataSet
                        : builder.SelectParameter + "," + dataSet;
                }
                else
                {
                    //throw new Exception("Experimental code exception");
                    return false;
                    //return ExecuteMethod(methodCall, builder, resultLoader, intermediateResultLoader);
                    //GetResult(methodCall, builder, resultLoader, intermediateResultLoader);
                    //The returned member property is not something the webservice
                    //handles, we need to pull the results
                    //then compose a list of that type
                }
            }


            //var propertyExpression = lambdaExpression.Body as MemberExpression;
            //if (propertyExpression != null)
            //{
            //    builder.SelectParameter = string.IsNullOrWhiteSpace(builder.SelectParameter)
            //        ? propertyExpression.Member.Name
            //        : builder.SelectParameter + "," + propertyExpression.Member.Name;
            //}

            return true;
        }*/


        private object ResolveProjection(QueryBuilder builder, LambdaExpression lambdaExpression, Type sourceType)
        {

            var selectFunction = lambdaExpression.Body as NewExpression;

            if (selectFunction != null)
            {
                var properties = sourceType.GetProperties();
                var members = selectFunction.Members
                    .Select(x => properties.FirstOrDefault(y => y.Name == x.Name) ?? x)
                                         .Select(x => _memberNameResolver.ResolveName(x))
                                         .ToArray();
                var args = selectFunction.Arguments.OfType<MemberExpression>()
                                         .Select(x => properties.FirstOrDefault(y => y.Name == x.Member.Name) ?? x.Member)
                                         .Select(x => _memberNameResolver.ResolveName(x))
                                         .ToArray();
                if (members.Intersect(args).Count() != members.Length)
                {
                    throw new InvalidOperationException("Projection into new member names is not supported.");
                }

                builder.SelectParameter = string.Join(",", args);
            }

            var propertyExpression = lambdaExpression.Body as MemberExpression;
            if (propertyExpression != null)
            {
                builder.SelectParameter = string.IsNullOrWhiteSpace(builder.SelectParameter)
                    ? _memberNameResolver.ResolveName(propertyExpression.Member)
                    : builder.SelectParameter + "," + _memberNameResolver.ResolveName(propertyExpression.Member);
            }

            return null;
        }
        /*
        private void resolveMapping(Expression arg0, Expression arg1, bool mapRoot = false)
        {

            if (mapRoot)
            {
                var returnRootSelection = GetLambda(arg1);
                quickResolution.Add(returnRootSelection.Parameters[0].Name, null);
                //var rootExpr = (NewExpression)returnRootSelection.Body;
                //if (rootExpr.Arguments.Count == 2)
                //{
                //    //Assume the second value in the anonymous type is the alias for our member
               //     quickResolution.Add(((ParameterExpression)rootExpr.Arguments[0]).Name, null);
                //}
               // else
                //{
                //    throw new Exception("Unexpected arguments list");
               // }
                return;
            }

            var lambdaExpr = GetLambda(arg0);
            var memberName = ((MemberExpression)lambdaExpr.Body).Member.Name;

            if (((MemberExpression)lambdaExpr.Body).Expression != null)
            {
                var paramExpr = ((MemberExpression)lambdaExpr.Body).Expression as MemberExpression;
                if (paramExpr != null)
                {
                    var prefix = paramExpr.Member.Name;
                    if (quickResolution.ContainsKey(prefix))
                    {
                        memberName = quickResolution[prefix] + "/" + memberName;
                    }
                    else
                    {
                        throw new Exception("Unable to resolve declaration tree");
                    }
                }
            }

            var returnSelection = GetLambda(arg1);
            if (!quickResolution.ContainsKey(returnSelection.Parameters[1].Name))
                quickResolution.Add(returnSelection.Parameters[1].Name, memberName);
            var newExpr = (NewExpression)returnSelection.Body;
           // if (newExpr.Arguments.Count == 2)
           // {
                //Assume the second value in the anonymous type is the alias for our member
           //     quickResolution.Add(((ParameterExpression)newExpr.Arguments[1]).Name,memberName);
          //  }
           // else
            //{
           //     throw new Exception("Unexpected arguments list");
            //}
            //if (returnSelection.Body.arg

            //lambdaExpr.Body
        }*/
        private static LambdaExpression GetLambda(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }
            if (e.NodeType == ExpressionType.Constant)
            {
                return ((ConstantExpression)e).Value as LambdaExpression;
            }
            return e as LambdaExpression;
        }
        private static object InvokeEager(MethodCallExpression methodCall, object source)
        {
            //Contract.Requires(source != null);
            //Contract.Requires(methodCall != null);

            var results = source as IEnumerable;

            //Contract.Assume(results != null);

            var parameters = ResolveInvocationParameters(results, methodCall);
            return methodCall.Method.Invoke(null, parameters);
        }

        private static object[] ResolveInvocationParameters(IEnumerable results, MethodCallExpression methodCall)
        {
            //Contract.Requires(results != null);
            //Contract.Requires(methodCall != null);

            var parameters = new object[] { results.AsQueryable() }
                .Concat(methodCall.Arguments.Where((x, i) => i > 0).Select(GetExpressionValue))
                .Where(x => x != null)
                .ToArray();
            return parameters;
        }

        private static object GetExpressionValue(Expression expression)
        {
            if (expression is UnaryExpression)
            {
                return (expression as UnaryExpression).Operand;
            }

            if (expression is ConstantExpression)
            {
                return (expression as ConstantExpression).Value;
            }

            return null;
        }

        private object GetMethodResult<T>(MethodCallExpression methodCall, QueryBuilder builder, Func<QueryBuilder, bool, IEnumerable<T>> resultLoader, Func<Type, QueryBuilder, IEnumerable> intermediateResultLoader)
        {
            //Contract.Requires(methodCall != null);
            //Contract.Requires(builder != null);
            //Contract.Requires(resultLoader != null);
            //Contract.Requires(intermediateResultLoader != null);
            //Contract.Assume(methodCall.Arguments.Count >= 2);

            ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);

            var processResult = _writer.Write(methodCall.Arguments[1], builder.SourceType);
            var currentParameter = string.IsNullOrWhiteSpace(builder.WhereParameter)
                                    ? processResult
                                    : string.Format("({0}) and ({1})", builder.WhereParameter, processResult);
            builder.WhereParameter = currentParameter;

            var genericArguments = methodCall.Method.GetGenericArguments();
            var queryableMethods = typeof(Queryable).GetMethods();

            //Contract.Assume(queryableMethods.Count() > 0);

            var nonGenericMethod = queryableMethods
                .Single(x => x.Name == methodCall.Method.Name && x.GetParameters().Length == 1);

            //Contract.Assume(nonGenericMethod != null);

            var method = nonGenericMethod
                .MakeGenericMethod(genericArguments);

            var list = resultLoader(builder, true);

            //Contract.Assume(list != null);

            var queryable = list.AsQueryable();
            var parameters = new object[] { queryable };
            var result = method.Invoke(null, parameters);
            return result ?? default(T);
        }

        private object GetResult<T>(MethodCallExpression methodCall, QueryBuilder builder, Func<QueryBuilder, bool, IEnumerable<T>> resultLoader, Func<Type, QueryBuilder, IEnumerable> intermediateResultLoader)
        {
            //Contract.Requires(builder != null);
            //Contract.Requires(methodCall != null);
            //Contract.Requires(resultLoader != null);
            //Contract.Requires(intermediateResultLoader != null);

            //Contract.Assume(methodCall.Arguments.Count >= 1);

            var data = ProcessMethodCall(methodCall.Arguments[0] as MethodCallExpression, builder, resultLoader, intermediateResultLoader);
            IEnumerable results;
            if (data == null)
            {
                results = resultLoader(builder, true);
            }
            else
            {
                results = (IEnumerable)data;
            }

            //Contract.Assume(results != null);

            var parameters = ResolveInvocationParameters(results, methodCall);
            try
            {
                var final = methodCall.Method.Invoke(null, parameters);
                if (data != null && final == null)
                    resultLoader(builder, false);
                return final;
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                    throw ex.InnerException;
                else
                    throw ex;
            }

        }

        private object ExecuteMethod<T>(MethodCallExpression methodCall, QueryBuilder builder, Func<QueryBuilder, bool, IEnumerable<T>> resultLoader, Func<Type, QueryBuilder, IEnumerable> intermediateResultLoader)
        {
            //Contract.Requires(methodCall != null);
            //Contract.Requires(resultLoader != null);
            //Contract.Requires(intermediateResultLoader != null);
            //Contract.Requires(builder != null);
            //Contract.Assume(methodCall.Arguments.Count >= 2);

            var innerMethod = methodCall.Arguments[0] as MethodCallExpression;

            /* if (innerMethod == null)
             {
                 return null;
             }*/
            Type genericArgument = null;
            if (innerMethod != null)
            {
                var result = ProcessMethodCall(innerMethod, builder, resultLoader, intermediateResultLoader);
                if (result != null)
                {
                    return InvokeEager(innerMethod, result);
                }

                genericArgument = innerMethod.Method.ReturnType.GetGenericArguments()[0];
            }
            else
            {
                var expr = methodCall.Arguments[0] as ConstantExpression;
                genericArgument = expr.Type.GenericTypeArguments[0];

            }
            var type = typeof(T);
            var list = type != genericArgument
             ? intermediateResultLoader(genericArgument, builder)
             : resultLoader(builder, true);


            var arguments = ResolveInvocationParameters(list, methodCall);

            return methodCall.Method.Invoke(null, arguments);
        }


    }
}
