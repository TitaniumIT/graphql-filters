using GraphQL.DI;
using GraphQL.Execution;
using Microsoft.Extensions.DependencyInjection;
using nl.titaniumit.graphql.filters.execution;

namespace nl.titaniumit.graphql.filters;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection AddGraphQLFilters(this IServiceCollection services)
    {
        services.AddSingleton<IConfigureSchema, SchemaConfigFilters>();
        services.AddSingleton<ScalarConverterService>();
        services.AddSingleton(srv => 
            new ExecutionStrategyRegistration(new FilterExecutionStrategy(),GraphQLParser.AST.OperationType.Query)
            );
        return services;
    }
}
