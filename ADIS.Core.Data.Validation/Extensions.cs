using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Validation
{
    public static class StringExtensions
    {
        public static string FormatString(this string format, object[] inputs)
        {
            return StringExtensions.FormatString(format, inputs, null);
        }

        public static string FormatString(this string format, object[] inputs, IFormatProvider formatProvider)
        {
            StringBuilder sb = new StringBuilder();
            Type[] types = new Type[inputs.Length];
            for (int x = 0; x < inputs.Length; x++)
            {
                types[x] = inputs[x].GetType();
            }
            Regex reg = new Regex(@"({)([^}]+)(})", RegexOptions.IgnoreCase);
            MatchCollection mc = reg.Matches(format);
            int startIndex = 0;
            foreach (Match m in mc)
            {
                Group g = m.Groups[2]; //it's second in the match between { and }
                int length = g.Index - startIndex - 1;
                sb.Append(format.Substring(startIndex, length));

                string toGet = String.Empty;
                string toFormat = String.Empty;
                int formatIndex = g.Value.IndexOf(":"); //formatting would be to the right of a :
                if (formatIndex == -1) //no formatting, no worries
                {
                    toGet = g.Value;
                }
                else //pickup the formatting
                {
                    toGet = g.Value.Substring(0, formatIndex);
                    toFormat = g.Value.Substring(formatIndex + 1);
                }

                //first try properties
                Type retrievedType = null;
                object retrievedObject = null;
                for (int x = 0; x < inputs.Length; x++)
                {
                    PropertyInfo retrievedProperty = types[x].GetProperty(toGet);
                    
                    
                    if (retrievedProperty != null)
                    {
                        retrievedType = retrievedProperty.PropertyType;
                        retrievedObject = retrievedProperty.GetValue(inputs[x], null);
                        break;
                    }
                    else //try fields
                    {
                        FieldInfo retrievedField = types[x].GetField(toGet);
                        if (retrievedField != null)
                        {
                            retrievedType = retrievedField.FieldType;
                            retrievedObject = retrievedField.GetValue(inputs[x]);
                            break;
                        }
                    }
                }
                if (retrievedType != null) //Cool, we found something
                {
                    string result = String.Empty;
                    if (toFormat == String.Empty) //no format info
                    {
                        if (retrievedObject == null)
                            result = "null";
                        else
                        result = retrievedType.InvokeMember("ToString",
                          BindingFlags.Public | BindingFlags.NonPublic |
                          BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase
                          , null, retrievedObject, null) as string;
                    }
                    else //format info
                    {
                        if (retrievedObject == null)
                            result = "null";
                        else
                        result = retrievedType.InvokeMember("ToString",
                          BindingFlags.Public | BindingFlags.NonPublic |
                          BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase
                          , null, retrievedObject, new object[] { toFormat, formatProvider }) as string;
                    }
                    sb.Append(result);
                }
                else //didn't find a property with that name, so be gracious and put it back
                {
                    sb.Append("{");
                    sb.Append(g.Value);
                    sb.Append("}");
                }
                startIndex = g.Index + g.Length + 1;
            }
            if (startIndex < format.Length) //include the rest (end) of the string
            {
                sb.Append(format.Substring(startIndex));
            }
            return sb.ToString();
        }
    }
    public static class ExpressionExtensions
    {
        public static Func<TSource, TResult> NullSafeEval<TSource, TResult>(this Expression<Func<TSource, TResult>> expression)
        {
            var safeExp = Expression.Lambda<Func<TSource, TResult>>(
                NullSafeEvalWrapper(expression.Body, typeof(TResult)),
                expression.Parameters[0]);

            var safeDelegate = safeExp.Compile();
            return safeDelegate;
        }

        private static Expression NullSafeEvalWrapper(Expression expr, Type type)
        {
            Expression obj;
            Expression safe = expr;

            while (!IsNullSafe(expr, out obj))
            {
                var isNull = Expression.Equal(obj, Expression.Constant(null));

                safe =
                    Expression.Condition
                    (
                        isNull,
                        Expression.Default(type),
                        safe
                    );

                expr = obj;
            }
            return safe;
        }

        private static bool IsNullSafe(Expression expr, out Expression nullableObject)
        {
            nullableObject = null;

            if (expr is MemberExpression || expr is MethodCallExpression)
            {
                Expression obj;
                MemberExpression memberExpr = expr as MemberExpression;
                MethodCallExpression callExpr = expr as MethodCallExpression;

                if (memberExpr != null)
                {
                    // Static fields don't require an instance
                    FieldInfo field = memberExpr.Member as FieldInfo;
                    if (field != null && field.IsStatic)
                        return true;

                    // Static properties don't require an instance
                    PropertyInfo property = memberExpr.Member as PropertyInfo;
                    if (property != null)
                    {
                        MethodInfo getter = property.GetGetMethod();
                        if (getter != null && getter.IsStatic)
                            return true;
                    }
                    obj = memberExpr.Expression;
                }
                else
                {
                    // Static methods don't require an instance
                    if (callExpr.Method.IsStatic)
                        return true;

                    obj = callExpr.Object;
                }

                // Value types can't be null
                if (obj.Type.IsValueType)
                    return true;

                // Instance member access or instance method call is not safe
                nullableObject = obj;
                return false;
            }
            return true;
        }
    }
}
