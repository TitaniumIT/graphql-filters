using GraphQL.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace nl.titaniumit.graphql.filters
{
    public static class SchemaExtentions
    {
        public static IServiceCollection AddGraphQLFilters(this IServiceCollection services)
        {
            services.AddSingleton<IValidationRule, DomainValidators>();
                return  services;
        }
    }
}
