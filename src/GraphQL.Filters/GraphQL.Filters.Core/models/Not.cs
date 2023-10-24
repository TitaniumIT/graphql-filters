using System.Linq.Expressions;

namespace nl.titaniumit.graphql.filters.models;

internal record Not(FilterType filter)
{
    internal Expression CreateFilter<T>(Expression arg) => Expression.Not(filter.CreateExpression<T>(arg));
}
