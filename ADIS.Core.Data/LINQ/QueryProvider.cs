using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.LINQ
{
    internal abstract class QueryProvider<T> : QueryProviderBase
    {
        private readonly IExpressionProcessor _expressionProcessor;
        private readonly QueryBuilder _queryBuilder;

         public QueryProvider(IExpressionProcessor expressionProcessor, INameResolver nameresolver, Type sourceType)
        {
            //Contract.Requires<ArgumentNullException>(client != null);
            //Contract.Requires<ArgumentNullException>(serializerFactory != null);
            //Contract.Requires<ArgumentNullException>(expressionProcessor != null);
            _expressionProcessor = expressionProcessor;
            _queryBuilder = new QueryBuilder(nameresolver, sourceType ?? typeof(T));
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Cannot dispose here.")]
        public override IQueryable CreateQuery(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            //TODO CHECK THIS CALLSTACK
            return null;
            //return new RestGetQueryable<T>(Client, SerializerFactory, expression);
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Cannot dispose here.")]
        public override IQueryable<TResult> CreateQuery<TResult>(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            //TODO CHECK THIS CALLSTACK
            return (IQueryable<TResult>)new SqlQueryable<TResult>(expression);
        }

        public override object Execute(Expression expression)
        {
            //Contract.Assume(expression != null);

            var methodCallExpression = expression as MethodCallExpression;
            var resultsLoaded = false;
            Func<QueryBuilder, bool, IEnumerable<T>> loadFunc = (p, d) =>
            {
                resultsLoaded = true;
                if (!d)
                    return null;
                return GetResults(p);
            };

            if (methodCallExpression != null)
            {

                var result = _expressionProcessor.ProcessMethodCall(methodCallExpression, _queryBuilder, loadFunc, GetIntermediateResults);
                if (result == null && resultsLoaded)
                    return null;
                else if (result == null)
                    return GetResults(_queryBuilder);
                else
                    return result;
            }
            else
            {
                var result = GetResults(_queryBuilder);
                if (result == null && resultsLoaded)
                    return null;
                else if (result == null)
                    return GetResults(_queryBuilder);
                else
                    return result;
            }

        }

        protected abstract IEnumerable<T> GetResults(QueryBuilder builder);

        protected abstract IEnumerable GetIntermediateResults(Type type, QueryBuilder builder);

    }
}
