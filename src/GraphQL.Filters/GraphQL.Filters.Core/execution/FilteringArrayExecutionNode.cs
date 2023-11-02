using GraphQL.Execution;
using GraphQL.Types;
using GraphQLParser.AST;

namespace nl.titaniumit.graphql.filters.execution;

internal class FilteringArrayExecutionNode : ArrayExecutionNode
{
    internal bool HasSubFilter {get;set;} = false;
    public FilteringArrayExecutionNode(ExecutionNode parent, IGraphType graphType, GraphQLField field, FieldType fieldDefinition, int? indexInParentNode) : base(parent, graphType, field, fieldDefinition, indexInParentNode)
    {
    }
    public override bool PropagateNull()
    {
        if( HasSubFilter)
        {
          if (  Items != null && ! Items.Any()  && Parent is FilteringObjectExecutionNode parentObject )
          {
                Items = null;
                Result = null;
                return true;
          }
        }
        var result = base.PropagateNull();
        
        Items = Items?.Where( i => i.Result != null).ToList() ?? null;

        return result;
    }
}
