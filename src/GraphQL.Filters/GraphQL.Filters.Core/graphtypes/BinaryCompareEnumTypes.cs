using GraphQL;
using GraphQL.Types;
using nl.titaniumit.graphql.filters.models;
using System.Linq.Expressions;

namespace nl.titaniumit.graphql.filters.graphtypes;

public class BinaryCompareEnumTypes : EnumerationGraphType
{
    public BinaryCompareEnumTypes()
    {
        Add("equal", new BinaryCompareType(ExpressionType.Equal));
        Add("greater", new BinaryCompareType(ExpressionType.GreaterThan));
        Add("greaterOrEqual", new BinaryCompareType(ExpressionType.GreaterThanOrEqual));
        Add("less", new BinaryCompareType(ExpressionType.LessThan));
        Add("lessOrEqual", new BinaryCompareType(ExpressionType.LessThanOrEqual));
        Add("notEqual", new BinaryCompareType(ExpressionType.NotEqual));
    }
}
