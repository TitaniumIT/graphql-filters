using GraphQL.DI;
using GraphQL.Types;
using nl.titaniumit.graphql.filters.visitors;

namespace nl.titaniumit.graphql.filters;

internal class SchemaConfigFilters : IConfigureSchema
{
    public void Configure(ISchema schema, IServiceProvider serviceProvider)
    {
        schema.RegisterVisitor(new FilterConfigVistor(serviceProvider));
    }
}
