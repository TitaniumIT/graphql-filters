using GraphQL.Builders;
using GraphQL.Types;
using nl.titaniumit.graphql.filters.graphtypes;

namespace nl.titaniumit.graphql.filters;

public class FilterBuilder<TSourceType, TReturnType>
{
    private readonly FieldBuilder<TSourceType, TReturnType> _parentBuilder;
    private readonly string _argumentName;
    Action<QueryArgument>? _configure;

    internal FilterBuilder(FieldBuilder<TSourceType, TReturnType> parentBuilder, string argumentName, Action<QueryArgument>? configure = null)
    {
        _parentBuilder = parentBuilder;
        _argumentName = argumentName;
        _configure = configure;
    }

    public FilterBuilder<TSourceType,TReturnType> ActAsSubFilter()
    {
        _parentBuilder.Configure( field =>{
            field.UpdateFieldOptions((config) => { config.ActAsSubFilter = true; });
        });
        return this;
    }

    public FieldBuilder<TSourceType, TReturnType> FilterType<TFilterType>() where TFilterType : class
    {
        _parentBuilder.Argument<FilterGraphType<TFilterType>>(_argumentName, _configure);
        return _parentBuilder;
    }
}
