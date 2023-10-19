using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace nl.titaniumit.graphql.filters.models;

internal record FilterType(ConditionType? condition=null, And? and=null, Or? or=null, Not? not=null)
{
    internal Expression<Func<T, bool>> CreateFilter<T>()
    {
        var arg = Expression.Parameter(typeof(T), "_this");
        return Expression.Lambda<Func<T, bool>>(
                 CreateFilter<T>(arg),
                 arg
                 );
    }

    internal bool IsValid()
    {
        return this switch {
            {condition: not null, and:null, or:null, @not:null} => true,
            {condition: null,and: not null,or:null,@not:null} => true,
            {condition: null,and:null,or:not null,@not:null} => true,
            {condition: null,and:null,or:null, @not: not null} => true,
              _ => false
        };
    }

    internal Expression CreateFilter<T>(ParameterExpression arg)
    { 
        return this switch {
            {condition: not null} => condition.CreateFilter<T>(arg),
            {and: not null }=> and.CreateFilter<T>(arg),
            {or: not null } =>  or.CreateFilter<T>(arg),
            {@not: not null } =>  not.CreateFilter<T>(arg),
             _ => throw new InvalidOperationException()
        };
    }
}
