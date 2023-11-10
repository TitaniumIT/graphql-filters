using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using GraphQL;

namespace nl.titaniumit.graphql.filters.models;

internal record FilterType(ConditionType? condition = null,
                        And? and = null, Or? or = null,
                        Not? not = null, Any? any = null,
                        IReadOnlyList<ConditionType>? ands = null,
                        IReadOnlyList<ConditionType>? ors = null)
{
    internal Expression<Func<T, bool>> CreateFilter<T>(IResolveFieldContext ctx)
    {
        var arg = Expression.Parameter(typeof(T), "_this");
        return Expression.Lambda<Func<T, bool>>(
                 CreateExpression<T>(arg, ctx),
                 arg
                 );
    }

    internal bool IsValid()
    {
        return this switch
        {
            { condition: not null, and: null, or: null, @not: null, any: null, ands: null, ors: null } => true,
            { condition: null, and: not null, or: null, @not: null, any: null, ands: null, ors: null } => true,
            { condition: null, and: null, or: not null, @not: null, any: null, ands: null, ors: null } => true,
            { condition: null, and: null, or: null, @not: not null, any: null, ands: null, ors: null } => true,
            { condition: null, and: null, or: null, @not: null, any: not null, ands: null, ors: null } => true,
            { condition: null, and: null, or: null, @not: null, any: not null, ands: not null, ors: null } => true,
            { condition: null, and: null, or: null, @not: null, any: not null, ands: null, ors: not null } => true,
            { condition: null, and: not null, or: null, @not: null, any: not null, ands: not null, ors: null } => true,
            { condition: null, and: null, or: not null, @not: null, any: not null, ands: null, ors: not null } => true,
            _ => false
        };
    }

    Expression CreateAnds<T>(IReadOnlyList<ConditionType> andList , Expression arg, IResolveFieldContext ctx)
    {
        Expression rval = andList.First().CreateFilter<T>(arg,ctx);
            foreach ( var item in andList.Skip(1))
            {
                rval = Expression.AndAlso(rval,item.CreateFilter<T>(arg,ctx));
            }
            return rval;
    }

    Expression CreateOrs<T>(IReadOnlyList<ConditionType> orlist, Expression arg, IResolveFieldContext ctx)
    {
        Expression rval = orlist.First().CreateFilter<T>(arg,ctx);
            foreach ( var item in orlist.Skip(1))
            {
                rval = Expression.Or(rval,item.CreateFilter<T>(arg,ctx));
            }
            return rval;
    }

    internal Expression CreateExpression<T>(Expression arg, IResolveFieldContext ctx)
    {
        return this switch
        {
            { condition: not null } => condition.CreateFilter<T>(arg, ctx),
            { and: not null } => and.CreateFilter<T>(arg, ctx),
            { or: not null } => or.CreateFilter<T>(arg, ctx),
            { @not: not null } => not.CreateFilter<T>(arg, ctx),
            { any: not null } => any.CreateFilter<T>(arg, ctx),
            { ands: not null, and: null } => CreateAnds<T>(ands,arg,ctx),
            { ors: not null, or: null } => CreateOrs<T>(ors,arg,ctx),
            { ands: not null, and: not null } => Expression.And( and.CreateFilter<T>(arg,ctx) , CreateAnds<T>(ands,arg,ctx)),
            { ors: not null, or: not null } => Expression.Or( or.CreateFilter<T>(arg,ctx), CreateOrs<T>(ors,arg,ctx)),
            _ => throw new InvalidOperationException()
        };
    }
}
