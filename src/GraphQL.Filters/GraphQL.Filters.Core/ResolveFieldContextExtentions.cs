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

         public static Expression<Func<TFilterType, bool>>? GetSubFilterExpression<TFilterType>(this IResolveFieldContext fieldContext)
         {
            if ( fieldContext.UserContext.ContainsKey($"path:{fieldContext.ParentType.Name}.{fieldContext.FieldDefinition.Name}")){
               return  fieldContext.UserContext[$"path:{fieldContext.ParentType.Name}.{fieldContext.FieldDefinition.Name}"] as  Expression<Func<TFilterType, bool>>;
            }
            return null;
         }

        static internal string StringPath(this IResolveFieldContext context)
        {
            return string.Join('.',context.Path.Select(p => $"{p}"));
        }
    }
}
