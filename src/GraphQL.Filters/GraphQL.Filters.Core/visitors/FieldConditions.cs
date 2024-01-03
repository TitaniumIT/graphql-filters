using nl.titaniumit.graphql.filters.models;

namespace nl.titaniumit.graphql.filters;

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