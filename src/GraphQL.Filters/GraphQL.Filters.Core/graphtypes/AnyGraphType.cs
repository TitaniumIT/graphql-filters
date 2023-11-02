using System.ComponentModel.DataAnnotations;
using System.Reflection;
using GraphQL;
using GraphQL.Types;
using nl.titaniumit.graphql.filters.models;

namespace nl.titaniumit.graphql.filters.graphtypes;

internal class AnyGraphType<T> : InputObjectGraphType<Any> where T : class
{
    public AnyGraphType()
    {
        var type = typeof(T);
        Name = $"AnyGraphType{typeof(T).Name}";
        foreach (var member in this.FieldsAndProperties<T>())
        {
            var memberType = member.MemberType();
            if (memberType.IsEnumerable())
            {
                AddAnyField(member, memberType);
            }
        }
    }

    internal void AddAnyField(MemberInfo member, Type memberType)
    {
        var ft = base.AddField(FieldBuilderExtentions.CreateAnyField(member.Name,memberType.GenericTypeArguments.First()));
        ft.Metadata["collectionType"] = memberType.GenericTypeArguments.First();
        Metadata[member.Name.ToCamelCase()] = member;
    }

    public override object ParseDictionary(IDictionary<string, object?> value)
    {
        if (value.Count != 1)
        {
            throw new ValidationException("Only one subfield is allowed");
        }
        if( Metadata[value.First().Key].ToString().StartsWith("path:"))
        {
            return new Any(
            null,
            value.First().Value as FilterType ?? throw new NullReferenceException(),
            Fields.Single(f => f.Name == value.First().Key).Metadata["collectionType"] as Type  ?? throw new NullReferenceException(),
            Metadata[value.First().Key].ToString()
          );
        }
        else{
        return new Any(
          Metadata[value.First().Key] as MemberInfo ?? throw new NullReferenceException(),
          value.First().Value as FilterType ?? throw new NullReferenceException(),
          Fields.Single(f => f.Name == value.First().Key).Metadata["collectionType"] as Type  ?? throw new NullReferenceException(),
          null
          );
        }
    }
}
