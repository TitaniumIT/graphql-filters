using GraphQL.Introspection;

namespace nl.titaniumit.graphql.filters.extentions.sql.ast;

internal record Value(object? value) : AstNode
{
    internal override string ToSql() => 
        value switch {
            string => $"'{value}'",
            _ => $"{value??"NULL"}"
        };
    
}

