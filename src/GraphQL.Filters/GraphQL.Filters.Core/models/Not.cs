using System.Linq.Expressions;

namespace nl.titaniumit.graphql.filters.models;

internal record Not(FilterType filter)
{
    internal Expression CreateFilter<T>(ParameterExpression arg) => Expression.Not(filter.CreateFilter<T>(arg));
}
