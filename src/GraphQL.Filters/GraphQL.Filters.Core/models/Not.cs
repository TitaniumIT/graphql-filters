using System.Linq.Expressions;
using GraphQL;

namespace nl.titaniumit.graphql.filters.models;

internal record Not(FilterType filter)
{
    internal Expression CreateFilter<T>(Expression arg,IResolveFieldContext ctx) => Expression.Not(filter.CreateExpression<T>(arg,ctx));
}
