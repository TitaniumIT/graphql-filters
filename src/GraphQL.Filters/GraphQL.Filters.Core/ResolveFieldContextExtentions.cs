using GraphQL;
using GraphQL.Validation;
using nl.titaniumit.graphql.filters.models;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace nl.titaniumit.graphql.filters
{
    public static class ResolveFieldContextExtentions
    {
        public static Expression<Func<TFilterType, bool>>? GetFilterExpression<TFilterType>(this IResolveFieldContext fieldContext, string argumentName)
        {
            try
            {
                var filter = fieldContext.GetArgument<FilterType>(argumentName);
                return filter.CreateFilter<TFilterType>(fieldContext);
            }
            catch (ValidationException ex)
            {
                fieldContext.Errors.Add(new ValidationError(ex.Message));
                return null;
            }
        }
    }
}
