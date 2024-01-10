using GraphQL;
using nl.titaniumit.graphql.filters.extentions.sql.ast;
using nl.titaniumit.graphql.filters.models;

namespace nl.titaniumit.graphql.filters.extentions.sql;

internal class FilterVisitorSql : FilterVisitorBase
{
    Stack<AstNode> _context = new Stack<AstNode>();
    IResolveFieldContext _resolveFieldContext;
    ISqlMapConfig _sqlMapConfig;

    public FilterVisitorSql(IResolveFieldContext resolveFieldContext,ISqlMapConfig sqlMapConfig)
    {
        _resolveFieldContext  = resolveFieldContext;
        _sqlMapConfig = sqlMapConfig;
    }

    public string GetWhere()
    {
        if ( _context.Count != 1)
        {
            throw new InvalidOperationException("Filter tree is unballanced, or missing expressions");
        }
        else
        {
            return _context.Pop().ToSql();
        }
    }
    public override bool Visit(ConditionType condition)
    {
        _context.Push( new Expressions( 
               _sqlMapConfig.GetSqlName(condition.fieldName),
             new Value(condition.Value(_resolveFieldContext).Value),
             condition.@operator?.expressionType ?? throw new NullReferenceException("condition")
             ));

        return true;
    }

    public override bool Visit(And and)
    {
        var r= base.Visit(and);
        _context.Push( new Expressions(
            _context.Pop(),
            _context.Pop(),
            System.Linq.Expressions.ExpressionType.And
        ));
        return r;
    }

    public override bool Visit(Not not)
    {
        var result= base.Visit(not);
        _context.Push(new Unary(UnaryTypes.not,_context.Pop()));
        return result;
    }

    public override bool Visit(Or and)
    {
        var r= base.Visit(and);
        _context.Push( new Expressions(
            _context.Pop(),
            _context.Pop(),
            System.Linq.Expressions.ExpressionType.Or
        ));
        return r;
    }

    public override bool VisitAnds(IReadOnlyList<ConditionType> ands)
    {
        return base.VisitAnds(ands);
    }

    public override bool VisitOrs(IReadOnlyList<ConditionType> ands)
    {
        return base.VisitAnds(ands);
    }

    public override bool Visit(Any any)
    {
        return base.Visit(any);
    }
}


