using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.LINQ
{
    internal abstract class QueryProviderBase : IQueryProvider, IDisposable
    {
        public abstract IQueryable CreateQuery(Expression expression);

        public abstract IQueryable<TElement> CreateQuery<TElement>(Expression expression);

        public abstract object Execute(Expression expression);

        public TResult Execute<TResult>(Expression expression)
        {
            //Contract.Assume(expression != null);
            return (TResult)Execute(expression);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract void Dispose(bool disposing);
    }
}
