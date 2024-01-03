using System.Linq.Expressions;

namespace nl.titaniumit.graphql.filters.extentions.sql.ast;

internal record Expressions(AstNode left, AstNode right,ExpressionType operation) : AstNode
{
    private static ExpressionType[] _boolean = { ExpressionType.And,ExpressionType.Or,ExpressionType.AndAlso,ExpressionType.OrElse};
    internal override string ToSql() {

        if ( _boolean.Any( x => x == operation) ) 
        {
            return $"({left.ToSql()}) {SqlOperation} ({right.ToSql()})";
        }
        else
        {
           return $"{left.ToSql()} {SqlOperation} {right.ToSql()}";
        }
    } 

    private string SqlOperation => operation switch {
        ExpressionType.Equal => "=",
        ExpressionType.AndAlso => "and",
        ExpressionType.And => "and",
        _ => throw new NotImplementedException()
    };
    
}

