using System.Linq.Expressions;
using System.Reflection;
using GraphQL;
using Microsoft.Extensions.DependencyInjection;

namespace nl.titaniumit.graphql.filters.models;

internal record ConditionType(MemberInfo? fieldName = null, Func<ConditionType, Expression, IResolveFieldContext, Expression>? @operator = null, object? @value = null)
{
    public Expression CreateFilter<T>(Expression arg, IResolveFieldContext ctx) => @operator?.Invoke(this, arg, ctx) ?? throw new InvalidOperationException();
    internal ConstantExpression Value(IResolveFieldContext ctx)
    {
        ScalarConverterService scalarConverterService = ctx.RequestServices.GetRequiredService<ScalarConverterService>();
        return Expression.Constant( scalarConverterService.ConvertFromScalar(value, fieldName.MemberType()));
    }

    internal MemberExpression? GetMemberExpression(Expression arg, IResolveFieldContext ctx)
    {
        MemberExpression? memberExpression = null;
        if (fieldName != null)
        {
            if (fieldName is PropertyInfo propertyInfo)
            {
                memberExpression = Expression.Property(arg, propertyInfo);
            }
            if (fieldName is FieldInfo fieldInfo)
            {
                memberExpression = Expression.Field(arg, fieldInfo);
            }
        }
        return memberExpression;
    }

    internal bool IsValid()
    {
        return this switch
        {
            { fieldName: not null, @operator: not null } => true,
            _ => false
        };
    }
}
