using System;
using System.Linq;
using System.Linq.Expressions;

namespace nl.titaniumit.graphql.filters.models
{
    internal record FilterType(ConditionType? condition, And? and, Or? or, Not not)
    {
        internal Expression<Func<T, bool>> CreateFilter<T>()
        {
            var arg = Expression.Parameter(typeof(T), "_this");
            return Expression.Lambda<Func<T, bool>>(
                     CreateFilter<T>(arg),
                     arg
                     );
        }

        internal Expression CreateFilter<T>(ParameterExpression arg)
        {
            if (condition != null)
            {
                return condition.CreateFilter<T>(arg);
            }
            if (and != null)
            {
                return and.CreateFilter<T>(arg);
            }
            if (or != null)
            {
                return or.CreateFilter<T>(arg);
            }
            throw new InvalidOperationException();
        }
    }
}
