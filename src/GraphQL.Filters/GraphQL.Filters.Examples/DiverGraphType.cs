using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using nl.titaniumit.graphql.filters;

namespace GraphQL.Filters.Examples;

public class DiverGraphType : AutoRegisteringObjectGraphType<Diver>
{
    public DiverGraphType()
    {
        Field<ListGraphType<DiveGraphType>>("Dives")
            .Resolve( ctx => {
              var datasource = ctx.RequestServices!.GetRequiredService<IDives>();
              var expression = ctx.GetSubFilterExpression<Dive>();
              if( expression != null){
                return datasource.Dives.Where( d => d.Diver?.Id == ctx.Source.Id).Where(expression.Compile());
              } else{
                return datasource.Dives.Where( d => d.Diver?.Id == ctx.Source.Id);
              }
            });
    }

}
