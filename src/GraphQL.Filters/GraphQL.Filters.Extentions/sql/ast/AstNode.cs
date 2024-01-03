namespace nl.titaniumit.graphql.filters.extentions.sql.ast;


internal abstract record AstNode
{
    internal abstract string ToSql();
};

