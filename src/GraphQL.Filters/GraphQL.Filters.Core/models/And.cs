using System;
using System.Linq;
using System.Linq.Expressions;

namespace nl.titaniumit.graphql.filters.models;

internal record And(FilterType left, FilterType right)
{
    internal Expression CreateFilter<T>(ParameterExpression arg)
    {
        return Expression.And(
            left.CreateFilter<T>(arg),
            right.CreateFilter<T>(arg)
        );
    }
}


