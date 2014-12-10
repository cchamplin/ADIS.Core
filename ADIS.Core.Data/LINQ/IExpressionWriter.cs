using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.LINQ
{
    /// <summary>
    /// Defines the public interface for an expression visitor.
    /// </summary>
    public interface IExpressionWriter
    {
        /// <summary>
        /// Generates a string representation of the passed expression.
        /// </summary>
        /// <param name="expression">The <see cref="Expression"/> to visit.</param>
        /// <returns>A string value.</returns>
        string Write(Expression expression, Type sourceType);
    }
}
