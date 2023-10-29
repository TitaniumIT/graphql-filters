using GraphQL.DI;
using GraphQL.Types;

namespace nl.titaniumit.graphql.filters;

internal class ConfigFilters : IConfigureSchema
{
    public void Configure(ISchema schema, IServiceProvider serviceProvider)
    {
        schema.RegisterVisitor(new FilterConfigVistor(serviceProvider));
    }
}
