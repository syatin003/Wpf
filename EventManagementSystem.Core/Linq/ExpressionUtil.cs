using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EventManagementSystem.Core.Linq
{
    /// <summary>
    /// Expression helper class that allows easy parsing of expressions.
    /// </summary>
    public static class ExpressionUtil
    {
        /// <summary>
        /// Cache for the property names.
        /// </summary>
        private static readonly Dictionary<string, string> _propertyNameCache = new Dictionary<string, string>();

        /// <summary>
        /// Gets the name of the property from the expression.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns>
        /// The name of the property parsed from the expression or <c>null</c> if the property cannot be found.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="propertyExpression"/> is <c>null</c>.</exception>
        public static string GetPropertyName<TProperty>(Expression<Func<TProperty>> propertyExpression)
        {
            return GetPropertyName(propertyExpression, false);
        }

        /// <summary>
        /// Gets the name of the property from the expression.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="ignoreCache">if set to <c>true</c>, the cache will be ignored and the value will be determined again, even when the item is already in the cache.</param>
        /// <returns>
        /// The name of the property parsed from the expression or <c>null</c> if the property cannot be found.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="propertyExpression"/> is <c>null</c>.</exception>
        public static string GetPropertyName<TProperty>(Expression<Func<TProperty>> propertyExpression, bool ignoreCache)
        {
            Argument.IsNotNull("propertyExpression", propertyExpression);

            string propertyExpressionAsString = propertyExpression.ToString();

            if (!ignoreCache)
            {
                if (_propertyNameCache.ContainsKey(propertyExpressionAsString))
                {
                    return _propertyNameCache[propertyExpressionAsString];
                }
            }

            var body = propertyExpression.Body as MemberExpression;
            if (body == null)
            {
                return null;
            }

            string propertyName = body.Member.Name;
            if (!_propertyNameCache.ContainsKey(propertyExpressionAsString))
            {
                _propertyNameCache.Add(propertyExpressionAsString, propertyName);
            }

            return propertyName;
        }

        /// <summary>
        /// Gets the owner of the expression. For example if the expression <c>() => MyProperty</c>, the owner of the
        /// property will be returned.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns>The owner of the expression or <c>null</c> if the owner cannot be found.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="propertyExpression"/> is <c>null</c>.</exception>
        public static object GetOwner<TProperty>(Expression<Func<TProperty>> propertyExpression)
        {
            Argument.IsNotNull("propertyExpression", propertyExpression);

            var body = propertyExpression.Body as MemberExpression;
            if (body == null)
            {
                return null;
            }

            var constantExpression = body.Expression as ConstantExpression;

            //if ((constantExpression == null) && (body.Expression is MemberExpression))
            //{
            //    constantExpression = ((MemberExpression) body.Expression).Expression as ConstantExpression;
            //}

            return (constantExpression != null) ? constantExpression.Value : null;
        }

        #region ExpressionUtil classes
        /// <summary>
        /// Create a function delegate representing a unary operation
        /// </summary>
        /// <typeparam name="TArg1">The parameter type</typeparam>
        /// <typeparam name="TResult">The return type</typeparam>
        /// <param name="body">Body factory</param>
        /// <returns>Compiled function delegate</returns>
        public static Func<TArg1, TResult> CreateExpression<TArg1, TResult>(
            Func<Expression, UnaryExpression> body)
        {
            ParameterExpression inp = Expression.Parameter(typeof(TArg1), "inp");

            try
            {
                return Expression.Lambda<Func<TArg1, TResult>>(body(inp), inp).Compile();
            }
            catch (Exception ex)
            {
                string msg = ex.Message; // avoid capture of ex itself
                return delegate { throw new InvalidOperationException(msg); };
            }
        }

        /// <summary>
        /// Create a function delegate representing a binary operation
        /// </summary>
        /// <typeparam name="TArg1">The first parameter type</typeparam>
        /// <typeparam name="TArg2">The second parameter type</typeparam>
        /// <typeparam name="TResult">The return type</typeparam>
        /// <param name="body">Body factory</param>
        /// <returns>Compiled function delegate</returns>
        public static Func<TArg1, TArg2, TResult> CreateExpression<TArg1, TArg2, TResult>(
            Func<Expression, Expression, BinaryExpression> body)
        {
            return CreateExpression<TArg1, TArg2, TResult>(body, false);
        }

        /// <summary>
        /// Create a function delegate representing a binary operation
        /// </summary>
        /// <param name="castArgsToResultOnFailure">
        /// If no matching operation is possible, attempt to convert
        /// TArg1 and TArg2 to TResult for a match? For example, there is no
        /// "decimal operator /(decimal, int)", but by converting TArg2 (int) to
        /// TResult (decimal) a match is found.
        /// </param>
        /// <typeparam name="TArg1">The first parameter type</typeparam>
        /// <typeparam name="TArg2">The second parameter type</typeparam>
        /// <typeparam name="TResult">The return type</typeparam>
        /// <param name="body">Body factory</param>
        /// <returns>Compiled function delegate</returns>
        public static Func<TArg1, TArg2, TResult> CreateExpression<TArg1, TArg2, TResult>(
            Func<Expression, Expression, BinaryExpression> body, bool castArgsToResultOnFailure)
        {
            ParameterExpression lhs = Expression.Parameter(typeof(TArg1), "lhs");
            ParameterExpression rhs = Expression.Parameter(typeof(TArg2), "rhs");
            try
            {
                try
                {
                    return Expression.Lambda<Func<TArg1, TArg2, TResult>>(body(lhs, rhs), lhs, rhs).Compile();
                }
                catch (InvalidOperationException)
                {
                    if (castArgsToResultOnFailure && !(         // if we show retry                                                        
                            typeof(TArg1) == typeof(TResult) &&  // and the args aren't
                            typeof(TArg2) == typeof(TResult)))
                    { // already "TValue, TValue, TValue"...
                        // convert both lhs and rhs to TResult (as appropriate)
                        Expression castLhs = typeof(TArg1) == typeof(TResult) ?
                                (Expression)lhs :
                                (Expression)Expression.Convert(lhs, typeof(TResult));
                        Expression castRhs = typeof(TArg2) == typeof(TResult) ?
                                (Expression)rhs :
                                (Expression)Expression.Convert(rhs, typeof(TResult));

                        return Expression.Lambda<Func<TArg1, TArg2, TResult>>(
                            body(castLhs, castRhs), lhs, rhs).Compile();
                    }
                    else throw;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message; // avoid capture of ex itself
                return delegate { throw new InvalidOperationException(msg); };
            }
        }
        #endregion
    }
}
