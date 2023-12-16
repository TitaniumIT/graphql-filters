using System.ComponentModel.DataAnnotations;
using GraphQL.Types;
using nl.titaniumit.graphql.filters.models;

namespace nl.titaniumit.graphql.filters.graphtypes;

internal class FilterGraphType<T> : InputObjectGraphType<FilterType> where T : class
{
    public FilterGraphType(FilterSettings filterSettings)
    {
        Name = $"FilterGraphType{typeof(T).Name}";
        Field<ConditionGraphType<T>>("condition");
        Field<AndGraphType<T>>("and");
        Field<OrGraphType<T>>("or");
        Field<NotGraphType<T>>("not");
        Field<ListGraphType<NonNullGraphType<ConditionGraphType<T>>>>("ands");
        Field<ListGraphType<NonNullGraphType<ConditionGraphType<T>>>>("ors");
        if( this.HasCollectionMembers<T>() || filterSettings.AlwaysIncludeAny){
            Field<AnyGraphType<T>>("any");
        }
    }

    public override void Initialize(ISchema schema)
    {
        schema.Metadata[$"filter:{Name}"]=typeof(T);
        base.Initialize(schema);
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
