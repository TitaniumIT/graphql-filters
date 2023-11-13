using GraphQL;
using GraphQL.Types;
using System.Reflection;

namespace nl.titaniumit.graphql.filters.graphtypes;

public class FieldEnumerationGraphType<T> : EnumerationGraphType where T : class
{ 
    static MemberTypes[] _visbleTypes = { MemberTypes.Property, MemberTypes.Field };
    public FieldEnumerationGraphType(FilterSettings filterSettings)
    {
        Name = $"FieldEnumerationGraphType{typeof(T).Name}";
        var type = typeof(T) ;

        BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
        if ( filterSettings.FlattenHierarchy){
            flags |= BindingFlags.FlattenHierarchy;
        }
        var members = type.GetMembers(flags);
        foreach( var member in members.Where( mi => _visbleTypes.Contains( mi.MemberType)))
        {
            if( ! member.MemberType().IsEnumerable() ){
                Add(new EnumValueDefinition(member.Name.ToCamelCase(), member));
            }
        }
    }
}
