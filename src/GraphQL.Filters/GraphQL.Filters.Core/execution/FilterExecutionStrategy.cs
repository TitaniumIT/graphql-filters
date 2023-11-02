using GraphQL.Execution;
using GraphQL.Types;
using GraphQLParser.AST;

namespace nl.titaniumit.graphql.filters.execution;

internal class FilterExecutionStrategy : ParallelExecutionStrategy
{
    protected override ExecutionNode BuildExecutionNode(ExecutionNode parent, IGraphType graphType, GraphQLField field, FieldType fieldDefinition, int? indexInParentNode = null)
    {
        if (graphType is ListGraphType)
        {
            return new FilteringArrayExecutionNode(parent, graphType, field, fieldDefinition, indexInParentNode);
        }
        if (graphType is IObjectGraphType)
        {
            return new FilteringObjectExecutionNode(parent, graphType, field, fieldDefinition, indexInParentNode);
        }
        return base.BuildExecutionNode(parent, graphType, field, fieldDefinition, indexInParentNode);
    }

    protected override Task CompleteNodeAsync(GraphQL.Execution.ExecutionContext context, ExecutionNode node)
    {
        var path = $"path:{node.Parent.GraphType.Name}.{node.FieldDefinition.Name}";
        if (context.UserContext.ContainsKey(path))
        {
           if ( node is FilteringArrayExecutionNode arrayNode)
           {
                arrayNode.HasSubFilter = true;
                if ( arrayNode.Parent is FilteringObjectExecutionNode objectParent)
                {
                    objectParent.SubFieldsHaveSubFilters = true;
                }
           }
        }
        return base.CompleteNodeAsync(context, node);
    }
}
