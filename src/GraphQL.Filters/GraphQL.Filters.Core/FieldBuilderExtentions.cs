using GraphQL.Builders;
using GraphQL.Types;
using nl.titaniumit.graphql.filters.graphtypes;
using nl.titaniumit.graphql.filters.models;

namespace nl.titaniumit.graphql.filters;

public static class FieldBuilderExtentions
{
    public static FilterBuilder<TSourceType, TReturnType> AddFilter<TSourceType, TReturnType>(
            this FieldBuilder<TSourceType, TReturnType> builder, string argumentName, Action<QueryArgument>? configure = null) =>
        new FilterBuilder<TSourceType, TReturnType>(builder,argumentName,configure);

}

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

    public FieldBuilder<TSourceType, TReturnType> FilterType<TFilterType>() where TFilterType : class
    {
        _parentBuilder.Argument<FilterGraphType<TFilterType>>(_argumentName, _configure);
        return _parentBuilder;
    }
}
