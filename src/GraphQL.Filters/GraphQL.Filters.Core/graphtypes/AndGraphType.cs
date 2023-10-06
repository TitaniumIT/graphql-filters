using GraphQL.Types;
using nl.titaniumit.graphql.filters.models;
using System;
using System.Linq;

namespace nl.titaniumit.graphql.filters.graphtypes
{
    internal class AndGraphType<T> : InputObjectGraphType<And> where T : class
    {
        public AndGraphType()
        {
            Name = $"AndGraphType{typeof(T).Name}";
            Field<NonNullGraphType<FilterGraphType<T>>>("left");
            Field<NonNullGraphType<FilterGraphType<T>>>("right");
        }
    }
}
