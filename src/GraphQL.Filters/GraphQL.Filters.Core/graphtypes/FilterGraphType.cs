using GraphQL.Types;
using nl.titaniumit.graphql.filters.models;

namespace nl.titaniumit.graphql.filters.graphtypes;

internal class FilterGraphType<T> : InputObjectGraphType<FilterType> where T : class
{
    public FilterGraphType()
    {
        Name = $"FilterGraphType{typeof(T).Name}";
        Field<ConditionGraphType<T>>("condition");
        Field<AndGraphType<T>>("and");
        Field<OrGraphType<T>>("or");
        Field<NotGraphType<T>>("not");

        Description = "";
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
        if (!value.ContainsKey("not"))
        {
            value["not"] = null;
        }
        if (  base.ParseDictionary(value) is FilterType filter){
            if( filter.IsValid()){ 
                return filter;
            }
        }
        throw new Exception();
    }
}
