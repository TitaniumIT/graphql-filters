using System.ComponentModel.DataAnnotations;
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
        if( this.HasCollectionMembers<T>()){
            Field<AnyGraphType<T>>("any");
        }
        Description = "only use one of the fields and leave the rest empty. Don't combine";
    }

    public override object ParseDictionary(IDictionary<string, object?> value)
    {
        if (  base.ParseDictionary(value) is FilterType filter){
            if( filter.IsValid()){ 
                return filter;
            }
            else{
               throw new ValidationException($"{filter} is invalid try: {Description}");
            }
        }
        throw new InvalidOperationException("Filter is not found");
    }
}
