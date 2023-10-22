using System.Linq.Expressions;
using System.Reflection;

namespace nl.titaniumit.graphql.filters.models;

internal record ConditionType(MemberInfo? fieldName=null, Func<ConditionType, ParameterExpression, Expression>? @operator= null, object? @value = null, FilterType? filter= null)
{
    public Expression CreateFilter<T>(ParameterExpression arg) => @operator?.Invoke(this, arg) ?? throw new InvalidOperationException();
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

     internal bool IsValid()
    {
        return this switch {
            {fieldName: not null, @operator: not null, @filter:null} => true,
            {fieldName:  null, @operator: null, @filter:not null, value:null} => true,
              _ => false
        };
    }

}
