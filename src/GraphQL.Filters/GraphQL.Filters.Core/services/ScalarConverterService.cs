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
        if ( _customScalars.ContainsKey(expectedType))
        {
                return _customScalars[expectedType].ParseValue(value);
        }
        else {
            if ( expectedType.IsEnum )
            {
                return Enum.Parse(expectedType,value?.ToString()??throw new NullReferenceException());
            }
        }
        throw new InvalidCastException($"No conversion found for {value.GetType().Name} to {expectedType.Name}");
    }
}


