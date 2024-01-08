using System.Linq.Expressions;
using System.Reflection;
using System.Security.Permissions;
using GraphQL;
using GraphQL.Introspection;
using nl.titaniumit.graphql.filters.models;

namespace nl.titaniumit.graphql.filters;

internal record BinaryCompareType(ExpressionType expressionType,
 Func<ConditionType, Expression, IResolveFieldContext, Expression>? expression)
{

    internal BinaryCompareType(ExpressionType expressionType) : this(expressionType, GetFactory(expressionType))
    {

    }

    static Func<ConditionType, Expression, IResolveFieldContext, Expression>? GetFactory(ExpressionType expressionType) =>
         expressionType switch
         {
             ExpressionType.Equal => (ConditionType condition, Expression arg, IResolveFieldContext ctx) => Binary(condition, arg, ctx, Expression.Equal),
             ExpressionType.GreaterThan => (ConditionType condition, Expression arg, IResolveFieldContext ctx) => Binary(condition, arg, ctx, Expression.GreaterThan),
             ExpressionType.GreaterThanOrEqual => (ConditionType condition, Expression arg, IResolveFieldContext ctx) => Binary(condition, arg, ctx, Expression.GreaterThanOrEqual),
             ExpressionType.LessThan => (ConditionType condition, Expression arg, IResolveFieldContext ctx) => Binary(condition, arg, ctx, Expression.LessThan),
             ExpressionType.LessThanOrEqual => (ConditionType condition, Expression arg, IResolveFieldContext ctx) => Binary(condition, arg, ctx, Expression.LessThanOrEqual),
             ExpressionType.NotEqual => (ConditionType condition, Expression arg, IResolveFieldContext ctx) => Binary(condition, arg, ctx, Expression.NotEqual),
             _ =>  throw new NotImplementedException()
         };


    static Expression Binary(ConditionType condition, Expression arg, IResolveFieldContext ctx, Func<Expression, Expression, bool, MethodInfo?, BinaryExpression> _factory)
    {
        var val = condition.Value(ctx);
        if (val.Type.IsPrimitive || val.Type == typeof(string) || val.Type.IsValueType )
            return _factory.Invoke(condition.GetMemberExpression(arg, ctx) ?? throw new NullReferenceException(), val, false, null);
        else
           return condition.@operator?.expressionType switch {
                 ExpressionType.Equal => _factory.Invoke(condition.GetMemberExpression(arg, ctx) ?? throw new NullReferenceException(), val, false, _equals),
                _ =>    throw new NotImplementedException()
            }
         ;
    }

    static MethodInfo? _equals = typeof(object).GetMethod(nameof(object.Equals),new []{ typeof(object),typeof(object)});
}