using System.Globalization;
using GraphQL;
using GraphQL.DI;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using nl.titaniumit.graphql.filters.graphtypes;
using nl.titaniumit.graphql.filters.models;

namespace nl.titaniumit.graphql.filters;

public static partial class SchemaExtentions
{
    static internal Type ClrType(this IObjectGraphType objectType )
    {
        if ( objectType != null && (objectType.GetType().BaseType?.GenericTypeArguments.Any() ??false))
        {
            return objectType.GetType().BaseType!.GenericTypeArguments.First();
        }
        return typeof(object);
    }

    static internal bool IsFilterType(this ISchema schema,Type type)
    {
        return schema.Metadata.Any( k=> k.Key.StartsWith("filter:") && (k.Value?.Equals(type)??false));
    }

    static public void AddFilterTypes<TType>(this ISchema schema) where TType : class
    {
        schema.RegisterType<FilterGraphType<TType>>();
    }
}

