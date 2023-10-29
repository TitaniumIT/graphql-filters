using GraphQL.Types;
using System.Reflection;

namespace nl.titaniumit.graphql.filters.graphtypes;

public class FieldEnumerationGraphType<T> : EnumerationGraphType where T : class
{ 
    static MemberTypes[] _visbleTypes = { MemberTypes.Property, MemberTypes.Field };
    public FieldEnumerationGraphType()
    {
        Name = $"FieldEnumerationGraphType{typeof(T).Name}";
        var type = typeof(T) ;
        var members = type.GetMembers(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        foreach( var member in members.Where( mi => _visbleTypes.Contains( mi.MemberType)))
        {
            Add(new EnumValueDefinition(member.Name, member));
        }
    }
}
