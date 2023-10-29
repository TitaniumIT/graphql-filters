using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL.Filters.Examples;

public class DiverGraphType : AutoRegisteringObjectGraphType<Diver>
{
    public DiverGraphType()
    {
        Field<ListGraphType<DiveGraphType>>("Dives")
            .Resolve( ctx => {
              var datasource = ctx.RequestServices!.GetRequiredService<IDives>();
              return datasource.Dives.Where( d => d.Diver.Id == ctx.Source.Id);
            });
    }
}
