using nl.titaniumit.graphql.filters.models;

namespace nl.titaniumit.graphql.filters;

internal abstract class FilterVisitorBase
{
    virtual public bool Visit(And and)
    {
     Visit(and.left);
      Visit(and.right);
      return true;
    }

    virtual public bool VisitAnds(IReadOnlyList<ConditionType> ands)
    {
        foreach (var conditionType in ands) Visit(conditionType);
        return true;
    }

    virtual public bool VisitOrs(IReadOnlyList<ConditionType> ors)
    {
        foreach (var conditionType in ors) Visit(conditionType);
        return true;
    }

    virtual public bool Visit(FilterType filter)
    {
       return filter switch
        {
            { condition: not null } => Visit(filter.condition),
            { and: not null, ands: null } => Visit(filter.and),
            { or: not null, ors: null } => Visit(filter.or),
            { @not: not null } => Visit(filter.not),
            { any: not null } => Visit(filter.any),
            { ands: not null, and: null } => VisitAnds(filter.ands),
            { ors: not null, or: null } => VisitOrs(filter.ors),
            { ands: not null, and: not null } => VisitAnds(filter.ands) && Visit(filter.and),
            { ors: not null, or: not null } =>  VisitOrs(filter.ors) && Visit(filter.or),
            _ => throw new InvalidOperationException()
        };
    }

    virtual public bool Visit(ConditionType condition)
    {
        return true;
    }

    virtual public bool Visit(Or or)
    {
        return Visit(or.left) && Visit(or.right);
    }

    virtual public bool Visit(Not @not)
    {
      return  Visit(@not.filter);
    }

    virtual public bool Visit(Any any)
    { 
        return Visit(any.filter);
    }
}
