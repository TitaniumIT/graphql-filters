using nl.titaniumit.graphql.filters.models;

namespace nl.titaniumit.graphql.filters;

internal abstract class FilterVisitorBase
{
    virtual public bool Visit(And and)
    {
        return Visit(and.left) || Visit(and.right);
    }

    virtual public bool Visit(IReadOnlyList<ConditionType> ands)
    {
        foreach (var conditionType in ands) Visit(conditionType);
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
            { ands: not null, and: null } => Visit(filter.ands),
            { ors: not null, or: null } => Visit(filter.ors),
            { ands: not null, and: not null } => Visit(filter.ands) || Visit(filter.and),
            { ors: not null, or: not null } =>  Visit(filter.ors) || Visit(filter.or),
        };
    }

    virtual public bool Visit(ConditionType condition)
    {
        return true;
    }

    virtual public bool Visit(Or or)
    {
        return Visit(or.left) ||  Visit(or.right);
    }

    virtual public bool Visit(Not @not)
    {
      return  Visit(@not.filter);
    }

    public bool Visit(Any any)
    { 
        return Visit(any.filter);
    }
}


internal class FieldConditions : FilterVisitorBase
{
    Func<IFieldConditionDefinition, bool> _callback;
    public FieldConditions(Func<IFieldConditionDefinition, bool> callBack)
    {
        _callback = callBack;
    }

    public override bool Visit(ConditionType condition)
    {
       return _callback(condition);
    }
}