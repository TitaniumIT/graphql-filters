using GraphQL.Types;

namespace nl.titaniumit.graphql.filters;

internal class ScalarConverterService
{
    Dictionary<Type, ScalarGraphType> _customScalars = new();
    Dictionary<Type, Func<object?, object?>> _builtinScalars = new();

    public ScalarConverterService()
    {
        _builtinScalars.Add(typeof(DateTime), (object? value) => value is string str ? DateTime.Parse(str) : throw new InvalidCastException());
        _builtinScalars.Add(typeof(DateOnly), (object? value) => value is string str ? DateOnly.Parse(str) : throw new InvalidCastException());
        _builtinScalars.Add(typeof(TimeOnly), (object? value) => value is string str ? TimeOnly.Parse(str) : throw new InvalidCastException());
        _builtinScalars.Add(typeof(DateTime?), (object? value) => value is string str ? DateTime.Parse(str) : throw new InvalidCastException());
        _builtinScalars.Add(typeof(DateOnly?), (object? value) => value is string str ? DateOnly.Parse(str) : throw new InvalidCastException());
        _builtinScalars.Add(typeof(TimeOnly?), (object? value) => value is string str ? TimeOnly.Parse(str) : throw new InvalidCastException());
    }

    internal void Add(Type type, ScalarGraphType scalarGraphType)
    {
        if (!_customScalars.ContainsKey(type))
            _customScalars.Add(type, scalarGraphType);
    }
    internal object? ConvertFromScalar(object? value, Type expectedType)
    {
        if (value == null) return null;
        if (value.GetType().IsAssignableTo(expectedType))
        {
            return value;
        }
        if (_customScalars.ContainsKey(expectedType))
        {
            return _customScalars[expectedType].ParseValue(value);
        }
        if (expectedType.IsEnum)
        {
            return Enum.Parse(expectedType, value?.ToString() ?? throw new NullReferenceException());
        }
        if (_builtinScalars.ContainsKey(expectedType))
        {
            return _builtinScalars[expectedType](value);
        }
        throw new InvalidCastException($"No conversion found for {value.GetType().Name} to {expectedType.Name}");
    }
}

