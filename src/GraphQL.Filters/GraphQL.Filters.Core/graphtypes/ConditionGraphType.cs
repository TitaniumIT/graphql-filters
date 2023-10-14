using GraphQL.Types;
using nl.titaniumit.graphql.filters.models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace nl.titaniumit.graphql.filters.graphtypes
{
    internal class ConditionGraphType<T> : InputObjectGraphType<ConditionType> where T : class
    {
        public ConditionGraphType()
        {
            Name = $"ConditionGraphType{typeof(T).Name}";
            Field<FieldEnumerationGraphType<T>>("fieldName");
            Field<BinaryCompareEnumTypes, Func<MemberExpression?, object?, Expression>>("operator");
            Field<ValueScalar>("value");
            Field<FilterGraphType<T>>("filter");
        }

        public override object ParseDictionary(IDictionary<string, object?> value)
        {
            if (!value.ContainsKey("filter"))
            {
                value["filter"] = null;
            }
            if (!value.ContainsKey("fieldName"))
            {
                value["fieldName"] = null;
            }
            if (!value.ContainsKey("value"))
            {
                value["value"] = null;
            }
            return base.ParseDictionary(value);
        }
    }
}
