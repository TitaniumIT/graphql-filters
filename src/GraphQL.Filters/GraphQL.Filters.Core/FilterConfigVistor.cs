using GraphQL;
using GraphQL.Types;
using GraphQL.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace nl.titaniumit.graphql.filters;

internal class FilterConfigVistor : BaseSchemaNodeVisitor
{
    ScalarConverterService _converterService;
    public FilterConfigVistor(IServiceProvider serviceProvider)
    {
        _converterService = serviceProvider.GetRequiredService<ScalarConverterService>();
    }

    public override void VisitObject(IObjectGraphType type, ISchema schema)
    {
        base.VisitObject(type, schema);
    }

    public override void VisitObjectFieldDefinition(FieldType field, IObjectGraphType type, ISchema schema)
    {
        var graphType = field.ResolvedType?.GetNamedType();
        if (graphType != null &&
             graphType.IsLeafType() &&
             graphType.GetType().Assembly != typeof(ISchema).Assembly &&
             !schema.BuiltInTypeMappings.Any(x => x.graphType == graphType.GetType()))
        {
            var clrType = schema.TypeMappings.Where(x => x.graphType == graphType.GetType()).Select(x => x.clrType).Single();
            _converterService.Add(clrType, graphType as ScalarGraphType ?? throw new InvalidOperationException());
        }
        base.VisitObjectFieldDefinition(field, type, schema);
    }

}

