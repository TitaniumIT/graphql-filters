using System.Text;
using Dynamitey;
using GraphQL.Validation;
using GraphQLParser.AST;
using GraphQLParser.Visitors;
using TechTalk.SpecFlow.Infrastructure;

namespace GraphQL.Filters.Tests;

public class OperationWriter : ASTVisitor<OperationWriterContext>
{
    private readonly StringBuilder stringBuilder = new();
    Queue<char> _tabs = new();

    string _indent => new string(_tabs.ToArray());

    protected override ValueTask VisitOperationDefinitionAsync(GraphQLOperationDefinition operationDefinition, OperationWriterContext context)
    {
        stringBuilder.Append($"{operationDefinition.Operation} {operationDefinition.Name.Value} ");
        return base.VisitOperationDefinitionAsync(operationDefinition, context);
    }
    protected override ValueTask VisitFieldAsync(GraphQLField field, OperationWriterContext context)
    {
        stringBuilder.Append($"{_indent}{field.Name.Value}");
        return base.VisitFieldAsync(field, context);
    }

    protected override ValueTask VisitArgumentAsync(GraphQLArgument argument, OperationWriterContext context)
    {
        stringBuilder.Append($"{argument.Name.Value}:");
        return base.VisitArgumentAsync(argument, context);
    }

    protected override async ValueTask VisitArgumentsAsync(GraphQLArguments arguments, OperationWriterContext context)
    {
        stringBuilder.Append("(");
        await base.VisitArgumentsAsync(arguments, context);
        stringBuilder.Append(")");
    }

    protected override async ValueTask VisitObjectValueAsync(GraphQLObjectValue objectValue, OperationWriterContext context)
    {
        stringBuilder.AppendLine("{");
        _tabs.Enqueue(' ');
        await  base.VisitObjectValueAsync(objectValue, context);
        stringBuilder.AppendLine($"{_indent}}}");
        _tabs.Dequeue();
    }

    protected override ValueTask VisitObjectFieldAsync(GraphQLObjectField objectField, OperationWriterContext context)
    {
        stringBuilder.Append($"{_indent}{objectField.Name.Value}:");
        return base.VisitObjectFieldAsync(objectField, context);
    }

    protected override ValueTask VisitStringValueAsync(GraphQLStringValue stringValue, OperationWriterContext context)
    {
        stringBuilder.AppendLine($"{stringValue.Value}");
        return base.VisitStringValueAsync(stringValue, context);
    }

    protected override ValueTask VisitEnumValueAsync(GraphQLEnumValue enumValue, OperationWriterContext context)
    {
        stringBuilder.AppendLine($"{enumValue.Name.Value}");
        return base.VisitEnumValueAsync(enumValue, context);
    }

    protected override ValueTask VisitIntValueAsync(GraphQLIntValue intValue, OperationWriterContext context)
    {
        stringBuilder.AppendLine($"{intValue.Value}");
        return base.VisitIntValueAsync(intValue, context);
    }

    protected override async ValueTask VisitSelectionSetAsync(GraphQLSelectionSet selectionSet, OperationWriterContext context)
    {
        stringBuilder.AppendLine(" { ");
        _tabs.Enqueue(' ');
        await base.VisitSelectionSetAsync(selectionSet, context);
        stringBuilder.AppendLine($"{_indent}}}");
        _tabs.Dequeue();
    }

    public async ValueTask Write(ISpecFlowOutputHelper specFlowOutputHelper, GraphQLOperationDefinition definition)
    {
        await VisitAsync(definition, new OperationWriterContext());
        specFlowOutputHelper.WriteLine(stringBuilder.ToString());
    }
}


public class OperationWriterContext : IASTVisitorContext
{
    public CancellationToken CancellationToken => CancellationToken.None;
}