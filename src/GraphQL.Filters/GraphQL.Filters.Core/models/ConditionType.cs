using System.Linq.Expressions;
using System.Reflection;

namespace nl.titaniumit.graphql.filters.models;

internal record ConditionType(MemberInfo? fieldName=null, Func<ConditionType, Expression, Expression>? @operator= null, object? @value = null)
{
    public Expression CreateFilter<T>(Expression arg) => @operator?.Invoke(this, arg) ?? throw new InvalidOperationException();
    internal ConstantExpression Value => Expression.Constant(@value);
    internal MemberExpression? GetMemberExpression(Expression arg)
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
        return this switch {
            {fieldName: not null, @operator: not null} => true,
              _ => false
        };
    }

}
