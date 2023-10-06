using System;
using System.Linq;
using System.Linq.Expressions;

namespace nl.titaniumit.graphql.filters.models
{
    internal record ConditionType(string? fieldName, BinaryCompareTypes @operator, object? @value, FilterType? filter)
    {
        public Expression CreateFilter<T>(ParameterExpression arg)
        {
            if (fieldName != null)
            {
                switch (@operator)
                {
                    case BinaryCompareTypes.equal:

                        var prop = Expression.Property(arg, fieldName);
                        var val = Expression.Constant(@value);
                        var eq = Expression.Equal(prop, val);
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
