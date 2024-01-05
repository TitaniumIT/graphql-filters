namespace nl.titaniumit.graphql.filters.extentions.sql.ast;
internal record Unary(AstNode condition) : AstNode
{
    internal override string ToSql() => $"NOT {condition.ToSql()}";

}