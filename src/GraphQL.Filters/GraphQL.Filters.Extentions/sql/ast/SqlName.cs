namespace nl.titaniumit.graphql.filters.extentions.sql.ast;

internal record SqlName(string name,string? alias=null) : AstNode
{
        internal override string ToSql() => $"{(alias!=null?$"{alias}.":"")}[{name}]";
}

