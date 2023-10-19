using GraphQL;
using nl.titaniumit.graphql.filters.models;
using System.Linq.Expressions;

namespace nl.titaniumit.graphql.filters
{
    public static class ResolveFieldContextExtentions
    {
        public static  Expression<Func<TFilterType,bool>> GetFilterExpression<TFilterType>(this IResolveFieldContext fieldContext,string argumentName)
        {
            var filter = fieldContext.GetArgument<FilterType>(argumentName);
            return filter.CreateFilter<TFilterType>();
        }
    }
}
