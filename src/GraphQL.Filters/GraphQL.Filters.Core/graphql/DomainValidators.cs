using GraphQL.Validation;
using GraphQLParser.AST;

namespace nl.titaniumit.graphql.filters;

internal class DomainValidators : IValidationRule
{
    public ValueTask<INodeVisitor?> ValidateAsync(ValidationContext context)
    {
          return  ValueTask.FromResult<INodeVisitor?>( new DomainInputNodeVisitor());
    }
}

internal class DomainInputNodeVisitor : INodeVisitor
{
    public ValueTask EnterAsync(ASTNode node, ValidationContext context)
    {
         if( node is GraphQLArgument argument)
         {
            int i=0;
         }

         return ValueTask.CompletedTask;
    }

    public ValueTask LeaveAsync(ASTNode node, ValidationContext context)
    {
          if( node is GraphQLArgument argument)
         {
            int i=0;
         }
        return ValueTask.CompletedTask;
    }
}
