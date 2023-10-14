using GraphQL.Types;
using nl.titaniumit.graphql.filters.models;
using System;
using System.Linq;

namespace nl.titaniumit.graphql.filters.graphtypes
{
    internal class FilterGraphType<T> : InputObjectGraphType<FilterType> where T : class
    {
        public FilterGraphType()
        {
            Name = $"FilterGraphType{typeof(T).Name}";
            Field<ConditionGraphType<T>>("condition");
            Field<AndGraphType<T>>("and");
            Field<AndGraphType<T>>("or");
        }

        public override object ParseDictionary(IDictionary<string, object?> value)
        {
            if (!value.ContainsKey("and"))
            {
                value["and"] = null;
            }
              if (!value.ContainsKey("or"))
            {
                value["or"] = null;
            } 
            if (!value.ContainsKey("condition"))
            {
                value["condition"] = null;
            }
            return base.ParseDictionary(value);
        }
    }
}
