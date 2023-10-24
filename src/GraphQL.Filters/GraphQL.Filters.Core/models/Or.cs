using System.Linq.Expressions;

namespace nl.titaniumit.graphql.filters.models;

internal record Or(FilterType left, FilterType right)
{
    internal Expression CreateFilter<T>(Expression arg) => Expression.Or(
            left.CreateExpression<T>(arg),
            right.CreateExpression<T>(arg)
        );

}
