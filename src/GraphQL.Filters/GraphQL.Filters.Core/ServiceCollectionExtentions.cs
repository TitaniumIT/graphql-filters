using GraphQL.DI;
using Microsoft.Extensions.DependencyInjection;

namespace nl.titaniumit.graphql.filters;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection AddGraphQLFilters(this IServiceCollection services)
    {
        services.AddSingleton<IConfigureSchema, ConfigFilters>();
        services.AddSingleton<ScalarConverterService>();
        return services;
    }
}
