using System.Diagnostics;
using GraphQL;
using GraphQL.Builders;
using GraphQL.Types;
using nl.titaniumit.graphql.filters.graphtypes;

namespace nl.titaniumit.graphql.filters;

public static class FieldBuilderExtentions
{
    public static FilterBuilder<TSourceType, TReturnType> AddFilter<TSourceType, TReturnType>(
            this FieldBuilder<TSourceType, TReturnType> builder, string argumentName, Action<QueryArgument>? configure = null) =>
        new FilterBuilder<TSourceType, TReturnType>(builder,argumentName,configure);

    internal static FieldType CreateAnyField(string name, Type memberType)
    {
        return new FieldType()
        {
            Name = name.ToCamelCase(),
            Type = typeof(FilterGraphType<>).MakeGenericType(memberType), 
        };
    }
}
