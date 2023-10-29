using System.Linq.Expressions;
using System.Reflection;
using GraphQL;
using nl.titaniumit.graphql.filters.models;

namespace nl.titaniumit.graphql.filters;

internal record Any(MemberInfo member, FilterType filter, Type collectionType)
{
    static MethodInfo _createFilter = typeof(FilterType).GetMethod(nameof(FilterType.CreateFilter), BindingFlags.Instance|BindingFlags.NonPublic)
        ?? throw new NullReferenceException();

    static MethodInfo _any = 
            typeof(Enumerable).GetMethods(BindingFlags.Static|BindingFlags.Public)
            .Single( mi => mi.Name == nameof(Enumerable.Any) && mi.GetParameters().Count() == 2 );

    static Expression _null = Expression.Constant(null);
    static Expression _false = Expression.Constant(false);
      
    internal Expression CreateFilter<T>(Expression arg,IResolveFieldContext ctx)
    {
        var memberAccess = Expression.MakeMemberAccess(arg, member);
        var expression = _createFilter.MakeGenericMethod(collectionType).Invoke(filter,new object[]{ ctx}) as Expression;
        var anyCall = Expression.Call( _any.MakeGenericMethod(collectionType) , memberAccess ,  expression! );
        var nullTrap = Expression.Condition( Expression.Equal(memberAccess,_null) ,_false , anyCall);
        return Expression.IsTrue(nullTrap);
    }
}