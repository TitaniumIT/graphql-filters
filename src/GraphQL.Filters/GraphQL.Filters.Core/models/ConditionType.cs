using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace nl.titaniumit.graphql.filters.models
{
    internal record ConditionType(MemberInfo? fieldName, BinaryCompareTypes @operator, object? @value, FilterType? filter)
    {
        public Expression CreateFilter<T>(ParameterExpression arg)
        {
            if (fieldName != null)
            {
                MemberExpression? memberExpression = null;
                if ( fieldName is PropertyInfo propertyInfo )
                {
                    memberExpression = Expression.Property(arg, propertyInfo);
                }
                if (fieldName is FieldInfo fieldInfo)
                {
                    memberExpression = MemberExpression.Field(arg, fieldInfo);
                }
                switch (@operator)
                {
                    case BinaryCompareTypes.equal:

                        var val = Expression.Constant(@value);
                        var eq = Expression.Equal(memberExpression ?? throw new NullReferenceException(), val);
                        return eq;
                }
            }
            if (filter != null)
            {
                switch (@operator)
                {
                    case BinaryCompareTypes.not:

                        break;
                }
            }
            throw new InvalidOperationException();
        }
    }
}
