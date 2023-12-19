using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using GraphQL;
using GraphQL.Types;
using GraphQL.Utilities;
using Microsoft.Extensions.DependencyInjection;
using nl.titaniumit.graphql.filters.graphtypes;

namespace nl.titaniumit.graphql.filters.visitors;

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
        if (graphType != null && graphType.GetType().Assembly != typeof(ISchema).Assembly)
        {
            if (graphType.IsLeafType() &&
              !schema.BuiltInTypeMappings.Any(x => x.graphType == graphType.GetType()))
            {
                var clrType = schema.TypeMappings.Where(x => x.graphType == graphType.GetType()).Select(x => x.clrType).SingleOrDefault();
                if (clrType != null)
                {
                    _converterService.Add(clrType, graphType as ScalarGraphType ?? throw new InvalidOperationException());
                }
            }
            if ((field.Type?.IsAssignableTo(typeof(ListGraphType)) ?? false) && graphType is IObjectGraphType objecType)
            {
                if (schema.IsFilterType(type.ClrType()))
                {
                    var any = schema.AllTypes.SingleOrDefault(t => t.Name == $"AnyGraphType{type.ClrType().Name}") as IInputObjectGraphType;
                    if (any != null)
                    {
                        if (!type.ClrType().GetMembers(BindingFlags.Public | BindingFlags.Instance).Any(m => m.Name.ToCamelCase() == field.Name))
                        {
                            if ( !field.HasFilterArguments() ?? true)
                            {
                                CreateDynamicAnyField(field, type, schema, objecType, any);
                                field.UpdateFieldOptions((config) => { config.ActAsSubFilter = true; });
                            }
                        }
                        else
                        {
                            if ( !field.HasFilterArguments() ?? true)
                            {
                                field.UpdateFieldOptions((config) => { config.ActAsSubFilter = true; });
                            }
                        }
                    }
                }
            }
        }
        base.VisitObjectFieldDefinition(field, type, schema);
    }

  
    private static void CreateDynamicAnyField(FieldType field, IObjectGraphType type, ISchema schema, IObjectGraphType objecType, IInputObjectGraphType any)
    {
        var ft = FieldBuilderExtentions.CreateAnyField(field.Name, objecType.ClrType());
        ft.ResolvedType = schema.AllTypes.SingleOrDefault(t => t.Name == $"FilterGraphType{objecType.ClrType().Name}");
        ft.Metadata["collectionType"] = objecType.ClrType();
        any.AddField(ft);
        any.Metadata[field.Name] = $"path:{type.Name}.{field.Name}";
    }
}

