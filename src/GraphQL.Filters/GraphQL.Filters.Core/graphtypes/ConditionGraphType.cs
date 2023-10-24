using GraphQL.Types;
using nl.titaniumit.graphql.filters.models;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace nl.titaniumit.graphql.filters.graphtypes;

internal class ConditionGraphType<T> : InputObjectGraphType<ConditionType> where T : class
{
    public ConditionGraphType()
    {
        Name = $"ConditionGraphType{typeof(T).Name}";
        Field<FieldEnumerationGraphType<T>>("fieldName");
        Field<BinaryCompareEnumTypes, Func<MemberExpression?, object?, Expression>>("operator");
        Field<ValueScalar>("value");
        Description = "valid combinations are fieldname,operator,value";
    }

    public override object ParseDictionary(IDictionary<string, object?> value)
    {
        if (base.ParseDictionary(value) is ConditionType condition)
        {
            if (condition.IsValid())
            {
                return condition;
            }
            else{
               throw new ValidationException($"{condition} is invalid, try {Description}");
            }
        }
        throw new InvalidOperationException($"Condition is not found");
    }
}
