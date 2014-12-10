using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data.LINQ.Writers;

namespace ADIS.Core.Data.LINQ
{
    public class SqlQueryable<T> : QueryableBase<T>
    {
        private readonly SqlQueryProvider<T> _sqlQueryProvider;

        public SqlQueryable()
            : base()
        {
            var memberNameResolver = new MemberNameResolver();
            _sqlQueryProvider = new SqlQueryProvider<T>(new ExpressionProcessor(new ExpressionWriter(memberNameResolver, FragmentWriterType.SQL), memberNameResolver), memberNameResolver, typeof(T));
            Provider = _sqlQueryProvider;
            Expression = Expression.Constant(this);

        }

        public SqlQueryable(Expression expression)
            : this()
        {

            Expression = expression;
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    _sqlQueryProvider.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}
