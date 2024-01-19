using System.Diagnostics.CodeAnalysis;

namespace nl.titaniumit.graphql.filters.extentions.sql.ast;

[ExcludeFromCodeCoverage]
internal abstract record AstNode
{
    internal abstract string ToSql();
};

