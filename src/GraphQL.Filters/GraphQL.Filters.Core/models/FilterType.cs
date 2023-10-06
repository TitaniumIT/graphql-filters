using System;
using System.Linq;
using System.Linq.Expressions;

namespace nl.titaniumit.graphql.filters.models
{
    internal record FilterType(ConditionType? condition, And? and)
    {
        public Expression<Func<T, bool>> CreateFilter<T>()
        {
            if (condition != null)
            {
                var arg = Expression.Parameter(typeof(T), "_this");
                return Expression.Lambda<Func<T, bool>>(
                    condition.CreateFilter<T>(arg),
                    arg
                    );
            }
            throw new InvalidOperationException();
        }
    }
}
