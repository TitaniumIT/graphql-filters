using GraphQL.Execution;
using GraphQL.Types;
using GraphQLParser.AST;

namespace nl.titaniumit.graphql.filters.execution;

internal class FilteringObjectExecutionNode : ObjectExecutionNode
{
    internal bool SubFieldsHaveSubFilters {get;set;}
    public FilteringObjectExecutionNode(ExecutionNode parent, IGraphType graphType, GraphQLField field, FieldType fieldDefinition, int? indexInParentNode) : base(parent, graphType, field, fieldDefinition, indexInParentNode)
    {
    }
    public override bool PropagateNull()
    {
        if ( SubFieldsHaveSubFilters && SubFields != null)
        {
            foreach ( FilteringArrayExecutionNode subnode in SubFields.Where( d => d is FilteringArrayExecutionNode) )
            {
               if ( subnode != null && subnode.Items != null && ! subnode.Items.Any() && subnode.HasSubFilter )
               {
                  SubFields = null;
                  Result = null;
                  return true;
               }
            }
        }
        return base.PropagateNull();
    }
}