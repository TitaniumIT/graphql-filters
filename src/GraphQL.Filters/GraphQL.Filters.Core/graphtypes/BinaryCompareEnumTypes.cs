using GraphQL;
using GraphQL.Types;
using nl.titaniumit.graphql.filters.models;
using System.Linq.Expressions;

namespace nl.titaniumit.graphql.filters.graphtypes;

public class BinaryCompareEnumTypes : EnumerationGraphType
{
    public BinaryCompareEnumTypes()
    {
        Add("equal", (ConditionType condition, Expression arg,IResolveFieldContext ctx) => Binary(condition, arg, ctx, Expression.Equal));
        Add("greater", (ConditionType condition, Expression arg,IResolveFieldContext ctx) => Binary(condition, arg,ctx, Expression.GreaterThan));
        Add("greaterOrEqual", (ConditionType condition, Expression arg,IResolveFieldContext ctx) => Binary(condition, arg, ctx,Expression.GreaterThanOrEqual));
        Add("less", (ConditionType condition, Expression arg,IResolveFieldContext ctx) => Binary(condition, arg,ctx, Expression.LessThan));
        Add("lessOrEqual", (ConditionType condition, Expression arg,IResolveFieldContext ctx) => Binary(condition, arg,ctx, Expression.LessThanOrEqual));
        Add("notEqual",(ConditionType condition, Expression arg,IResolveFieldContext ctx) => Binary(condition, arg,ctx, Expression.NotEqual) );
    }

    static Expression Binary(ConditionType condition, Expression arg, IResolveFieldContext ctx,Func<Expression, Expression, BinaryExpression> _factory)
    {
        var val = condition.Value(ctx);
        var eq = _factory.Invoke(condition.GetMemberExpression(arg,ctx) ?? throw new NullReferenceException(), val);
        return eq;
    }
}
