using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using nl.titaniumit.graphql.filters;

namespace GraphQL.Filters.Examples;

public class DiverGraphType : AutoRegisteringObjectGraphType<Diver>
{
  public DiverGraphType() : base(x => x.Buddies)
  {
    Field<ListGraphType<DiveGraphType>>("Dives")
        .Resolve(ctx =>
        {
          var datasource = ctx.RequestServices!.GetRequiredService<IDives>();
          var expression = ctx.GetSubFilterExpression<IDive>();
          if (expression != null)
          {
            return datasource.Dives.Where(d => d.Diver?.Id == ctx.Source.Id).Where(expression.Compile());
          }
          else
          {
            return datasource.Dives.Where(d => d.Diver?.Id == ctx.Source.Id);
          }
        });

    Field<ListGraphType<DiverGraphType>>("Buddies")
        .Description("Using filters with ActAsSubFilter removes parents if child resolves to null")
        .AddFilter("filter")
          .ActAsSubFilter()
          .FilterType<Diver>()
        .Resolve(ctx =>
        {
          var filter = ctx.GetFilterExpression<Diver>("filter");
          if (filter != null)
          {
            return ctx.Source.Buddies.Where(filter.Compile()).ToList();
          }
          return ctx.Source.Buddies;
        });
  }

}
