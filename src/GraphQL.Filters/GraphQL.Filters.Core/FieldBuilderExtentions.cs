using GraphQL.Builders;
using GraphQL.Types;

namespace nl.titaniumit.graphql.filters;

public static class FieldBuilderExtentions
{
    public static FilterBuilder<TSourceType, TReturnType> AddFilter<TSourceType, TReturnType>(
            this FieldBuilder<TSourceType, TReturnType> builder, string argumentName, Action<QueryArgument>? configure = null) =>
        new FilterBuilder<TSourceType, TReturnType>(builder,argumentName,configure);

}
