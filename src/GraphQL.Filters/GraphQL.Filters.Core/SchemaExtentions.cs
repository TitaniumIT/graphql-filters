using GraphQL.Validation;
using Microsoft.Extensions.DependencyInjection;
using nl.titaniumit.graphql.filters.models;

namespace nl.titaniumit.graphql.filters;

public static class SchemaExtentions
{
    public static IServiceCollection AddGraphQLFilters(this IServiceCollection services)
    {
        return services;
    }
}
