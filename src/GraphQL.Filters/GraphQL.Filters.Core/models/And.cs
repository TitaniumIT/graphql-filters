using System.Linq.Expressions;

namespace nl.titaniumit.graphql.filters.models;

internal record And(FilterType left, FilterType right)
{
    internal Expression CreateFilter<T>(Expression arg) => Expression.And(
            left.CreateExpression<T>(arg),
            right.CreateExpression<T>(arg)
        );
}


