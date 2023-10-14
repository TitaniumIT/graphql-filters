using GraphQL.Types;
using nl.titaniumit.graphql.filters.models;

namespace nl.titaniumit.graphql.filters.graphtypes
{
    internal class OrGraphType<T> : InputObjectGraphType<Or> where T : class
    {
        public OrGraphType()
        {
            Name = $"OrraphType{typeof(T).Name}";
            Field<NonNullGraphType<FilterGraphType<T>>>("left");
            Field<NonNullGraphType<FilterGraphType<T>>>("right");
        }
    }
}
