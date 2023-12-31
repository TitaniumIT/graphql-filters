﻿using GraphQL;
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
                if (filter != null)
                {
                    return filter.CreateFilter<TFilterType>(fieldContext);
                }
                else
                {
                    return null;
                }
            }
            catch (ValidationException ex)
            {
                fieldContext.Errors.Add(new ValidationError(ex.Message));
                return null;
            }
        }

        public static Task VisitFilterConditions(this IResolveFieldContext fieldContext, string argumentName, Func<IFieldConditionDefinition, bool> conditionVisit)
        {
            var filter = fieldContext.GetArgument<FilterType>(argumentName);
            if (filter != null)
            {
               new FieldConditions(conditionVisit).Visit(filter);
            }
            return Task.CompletedTask;
        }

        public static Expression<Func<TFilterType, bool>>? GetSubFilterExpression<TFilterType>(this IResolveFieldContext fieldContext)
        {
            if (fieldContext.UserContext.ContainsKey($"path:{fieldContext.ParentType.Name}.{fieldContext.FieldDefinition.Name}"))
            {
                var expression = fieldContext.UserContext[$"path:{fieldContext.ParentType.Name}.{fieldContext.FieldDefinition.Name}"] as LambdaExpression;
                if (expression != null && expression.Parameters.First().Type == typeof(TFilterType))
                    return expression as Expression<Func<TFilterType, bool>>;
                else
                {
                    throw new NotSupportedException("yet");
                }
            }
            return null;
        }

        static internal string StringPath(this IResolveFieldContext context)
        {
            return string.Join('.', context.Path.Select(p => $"{p}"));
        }
    }
}
