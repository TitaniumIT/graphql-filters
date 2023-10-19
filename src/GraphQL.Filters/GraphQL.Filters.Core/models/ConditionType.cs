using System.Linq.Expressions;
using System.Reflection;

namespace nl.titaniumit.graphql.filters.models;

internal record ConditionType(MemberInfo? fieldName, Func<ConditionType, ParameterExpression, Expression> @operator, object? @value, FilterType? filter)
{
    public Expression CreateFilter<T>(ParameterExpression arg) => @operator.Invoke(this, arg);
    internal ConstantExpression Value => Expression.Constant(@value);
    internal MemberExpression? GetMemberExpression(ParameterExpression arg)
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
                memberExpression = MemberExpression.Field(arg, fieldInfo);
            }
        }
        return memberExpression;
    }
}
