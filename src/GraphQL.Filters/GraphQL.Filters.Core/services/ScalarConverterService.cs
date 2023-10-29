using GraphQL.Types;

namespace nl.titaniumit.graphql.filters;

internal class ScalarConverterService
{
    Dictionary<Type, ScalarGraphType> _customScalars = new();
    internal void Add(Type type, ScalarGraphType scalarGraphType) => _customScalars.Add(type, scalarGraphType);
    internal object? ConvertFromScalar(object? value, Type expectedType)
    {
        if (value == null) return null;
        if (value.GetType().IsAssignableTo(expectedType))
        {
            return value;
        }
        return _customScalars[expectedType].ParseValue(value);
    }
}
