using System.Linq.Expressions;
using System.Reflection;
using nl.titaniumit.graphql.filters.models;

namespace nl.titaniumit.graphql.filters;

internal record Any(MemberInfo member, FilterType filter, Type collectionType)
{
    static MethodInfo _createFilter = typeof(FilterType).GetMethod(nameof(FilterType.CreateFilter), BindingFlags.Instance|BindingFlags.NonPublic)
        ?? throw new NullReferenceException();

    static MethodInfo _any = 
            typeof(Enumerable).GetMethods(BindingFlags.Static|BindingFlags.Public)
            .Single( mi => mi.Name == nameof(Enumerable.Any) && mi.GetParameters().Count() == 2 );
      
    internal Expression CreateFilter<T>(Expression arg)
    {
        var memberAccess = Expression.MakeMemberAccess(arg, member);

        var expression = _createFilter.MakeGenericMethod(collectionType).Invoke(filter, Array.Empty<object?>()) as Expression;

        var anyCall = Expression.Call( _any.MakeGenericMethod(collectionType) , memberAccess ,  expression! );

        return Expression.IsTrue(anyCall);
    }
}