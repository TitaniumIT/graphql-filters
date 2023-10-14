using System.Linq.Expressions;

namespace nl.titaniumit.graphql.filters.models;

internal record Or(FilterType left, FilterType right)
{
    internal Expression CreateFilter<T>(ParameterExpression arg)
    {
        return Expression.Or(
            left.CreateFilter<T>(arg),
            right.CreateFilter<T>(arg)
        );
    }
}
