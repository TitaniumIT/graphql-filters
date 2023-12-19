using GraphQL.DataLoader;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using nl.titaniumit.graphql.filters;

namespace GraphQL.Filters.Examples;

public class DiverGraphType : AutoRegisteringObjectGraphType<Diver>
{
  public DiverGraphType(IDataLoaderContextAccessor accessor) : base(x => x.Buddies)
  {
    Field<ListGraphType<DiveGraphType>>("Dives")
        .Resolve(ctx =>
        {
          var datasource = ctx.RequestServices!.GetRequiredService<IDives>();
          var expression = ctx.GetSubFilterExpression<Dive>();
          if (expression != null)
          {
            return datasource.Dives.Where(d => d.Diver?.Id == ctx.Source.Id).Where(expression.Compile());
          }
          else
          {
            return datasource.Dives.Where(d => d.Diver?.Id == ctx.Source.Id);
          }
        });

    Field<ListGraphType<DiveGraphType>>("DivesWithDataLoader")
        .Description("Using a dataloader and subfilters")
        .ResolveAsync(async ctx =>
        {
          var datasource = ctx.RequestServices!.GetRequiredService<IDives>();
          var expression = ctx.GetSubFilterExpression<Dive>();

          var loader = accessor!.Context!.GetOrAddBatchLoader<int,List<Dive>>("dives", async (IEnumerable<int> ids , CancellationToken token)=>{
              if (expression != null)
              {
                return datasource.Dives
                      .Where(d => ids.ToList().Contains( d.Diver?.Id ?? -1 ))
                      .Where(expression.Compile()).Cast<Dive>()
                      .GroupBy(k => k.Diver!.Id)
                      .ToDictionary( k => k.Key , v => v.ToList());
              }
              else
              {
                return datasource.Dives
                      .Where(d => ids.ToList().Contains( d.Diver!.Id ))
                      .GroupBy(k => k.Diver!.Id)
                      .ToDictionary( k => k.Key , v => v.ToList());
              }
          });
         
         return loader.LoadAsync(ctx.Source.Id);
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

       Field<ListGraphType<DiverGraphType>>("BuddiesNoSubfilter")
        .Description("Using filters without ActAsSubFilter keeps parents if child resolves to null")
        .AddFilter("filter")
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
