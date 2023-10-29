using System.Collections;
using System.Reflection;
using GraphQL.Types;

namespace nl.titaniumit.graphql.filters;

public static class ReflectionExtentions
{
       static MemberTypes[] _visbleTypes = { MemberTypes.Property, MemberTypes.Field };

       public static IEnumerable<MemberInfo> FieldsAndProperties<T>(this IInputObjectGraphType _) where T : class
       {
          var members = typeof(T).GetMembers(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
          return members.Where(mi =>_visbleTypes.Contains(mi.MemberType));
       }

       public static Type MemberType(this MemberInfo member)
       {
         return member switch
            {
                PropertyInfo pi => pi.PropertyType,
                FieldInfo fi => fi.FieldType,
                _ => throw new InvalidOperationException()
            };
       }

       public static bool IsEnumerable(this Type type)
       {
         return type.IsAssignableTo( typeof(IEnumerable) ) && type.IsGenericType;
       }

       public static bool HasCollectionMembers<T>(this IInputObjectGraphType _) where T : class
       {
           return _.FieldsAndProperties<T>().Any( mi => mi.MemberType().IsEnumerable() );
       }

}
