using GraphQL.Types;
using GraphQLParser.AST;

namespace nl.titaniumit.graphql.filters.graphtypes;

public class ValueScalar : ScalarGraphType
{
    public override object? ParseValue(object? value)
    {
        return value;
    }
}
