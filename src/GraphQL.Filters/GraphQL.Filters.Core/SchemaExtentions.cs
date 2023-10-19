using Microsoft.Extensions.DependencyInjection;

namespace nl.titaniumit.graphql.filters;

public static class SchemaExtentions
{
    public static IServiceCollection AddGraphQLFilters(this IServiceCollection services)
    {
        return services;
    }
}
