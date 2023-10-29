using System.Linq.Expressions;
using GraphQL;

namespace nl.titaniumit.graphql.filters.models;

internal record Or(FilterType left, FilterType right)
{
    internal Expression CreateFilter<T>(Expression arg,IResolveFieldContext ctx) => Expression.Or(
            left.CreateExpression<T>(arg,ctx),
            right.CreateExpression<T>(arg,ctx)
        );

}
