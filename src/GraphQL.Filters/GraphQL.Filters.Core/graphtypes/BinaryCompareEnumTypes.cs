using GraphQL.Types;
using nl.titaniumit.graphql.filters.models;
using System.Linq.Expressions;

namespace nl.titaniumit.graphql.filters.graphtypes;

public class BinaryCompareEnumTypes : EnumerationGraphType
{
    public BinaryCompareEnumTypes()
    {
        Add("equal", (ConditionType condition, Expression arg) => Binary(condition, arg, Expression.Equal));
        Add("greater", (ConditionType condition, Expression arg) => Binary(condition, arg, Expression.GreaterThan));
        Add("greaterOrEqual", (ConditionType condition, Expression arg) => Binary(condition, arg, Expression.GreaterThanOrEqual));
        Add("less", (ConditionType condition, Expression arg) => Binary(condition, arg, Expression.LessThan));
        Add("lessOrEqual", (ConditionType condition, Expression arg) => Binary(condition, arg, Expression.LessThanOrEqual));
        Add("notEqual",(ConditionType condition, Expression arg) => Binary(condition, arg, Expression.NotEqual) );
    }

    static Expression Binary(ConditionType condition, Expression arg, Func<Expression, Expression, BinaryExpression> _factory)
    {
        var val = condition.Value;
        var eq = _factory.Invoke(condition.GetMemberExpression(arg) ?? throw new NullReferenceException(), val);
        return eq;
    }
}
