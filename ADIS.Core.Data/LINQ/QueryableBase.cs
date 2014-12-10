using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.LINQ
{
    public class QueryableBase<T> : IOrderedQueryable<T>, IDisposable, ISqlQueryableBase
    {
        public QueryableBase()
        {
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public Expression Expression { get; protected set; }
        public IQueryProvider Provider { get; protected set; }
        public IEnumerator<T> GetEnumerator()
        {

            //return Provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();
            var enumerable = Provider.Execute<IEnumerable<T>>(Expression);
            return (enumerable ?? new T[0]).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Provider.Execute<IEnumerable>(Expression).GetEnumerator();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }


        public Type Type
        {
            get { return typeof(T); }
        }
    }
}
