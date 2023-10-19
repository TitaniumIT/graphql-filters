using GraphQL.Types;
using nl.titaniumit.graphql.filters.models;

namespace nl.titaniumit.graphql.filters.graphtypes;

internal class NotGraphType<T> : InputObjectGraphType<Not> where T : class
{
    public NotGraphType()
    {
        Name = $"NotGraphType{typeof(T).Name}";
        Field<NonNullGraphType<FilterGraphType<T>>>("filter");
    }
}
