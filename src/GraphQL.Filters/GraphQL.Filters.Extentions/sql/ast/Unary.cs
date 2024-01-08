using System.Linq.Expressions;

namespace nl.titaniumit.graphql.filters.extentions.sql.ast;

internal enum UnaryTypes { not }
internal record Unary(UnaryTypes type, AstNode condition) : AstNode
{
    internal override string ToSql() => $"{Sql} {condition.ToSql()}";

    string Sql => type switch {
        UnaryTypes.not => "NOT",
        _ => throw new NotImplementedException($"{type}")
    };

}