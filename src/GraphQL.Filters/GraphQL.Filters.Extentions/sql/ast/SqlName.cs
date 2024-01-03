using System.Reflection;

namespace nl.titaniumit.graphql.filters.extentions.sql.ast;

internal record SqlName(string name,string? alias=null) : AstNode
{
    internal static AstNode GetSqlName(MemberInfo? fieldName)
    {
        throw new NotImplementedException();
    }

    internal override string ToSql() => $"{(alias!=null?$"{alias}.":"")}[{name}]";
}

