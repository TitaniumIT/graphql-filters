using GraphQL.DI;
using GraphQL.Execution;
using Microsoft.Extensions.DependencyInjection;
using nl.titaniumit.graphql.filters.execution;

namespace nl.titaniumit.graphql.filters;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection AddGraphQLFilters(this IServiceCollection services,FilterSettings? settings=null)
    {
        services.AddSingleton<IConfigureSchema, SchemaConfigFilters>();
        services.AddSingleton<ScalarConverterService>();
        if ( settings == null) settings= new(); 
        services.AddSingleton(settings);
        services.AddSingleton(srv => 
            new ExecutionStrategyRegistration(new FilterExecutionStrategy(),GraphQLParser.AST.OperationType.Query)
            );
        return services;
    }
}

public class FilterSettings {
    public bool FlattenHierarchy {get;set;}=false;
    public bool AlwaysIncludeAny {get;set;}=false;
}
