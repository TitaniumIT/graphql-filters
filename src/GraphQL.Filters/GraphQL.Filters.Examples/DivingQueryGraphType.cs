using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using nl.titaniumit.graphql.filters;

namespace GraphQL.Filters.Examples;

public class DivingQueryGraphType : ObjectGraphType
{
    public DivingQueryGraphType()
    {
        Field<DiverGraphType>("Diver")
          .AddFilter("filter").FilterType<Diver>()
          .Resolve(ctx =>
          {
              var filter = ctx.GetFilterExpression<Diver>("filter");
              var datasource = ctx.RequestServices!.GetRequiredService<IDivers>();
              if (filter != null)
                  return datasource.Divers.SingleOrDefault(filter.Compile());
              else
                  return null;
          });

         Field<ListGraphType<DiverGraphType>>("Divers")
          .AddFilter("filter").FilterType<Diver>()
          .Resolve(ctx =>
          {
              var filter = ctx.GetFilterExpression<Diver>("filter");
              var datasource = ctx.RequestServices!.GetRequiredService<IDivers>();
              if (filter != null)
                  return datasource.Divers.Where(filter.Compile());
              else
                  return null;
          });

        Field<NonNullGraphType<ListGraphType<NonNullGraphType<DiverGraphType>>>>("NonNullDivers")
          .AddFilter("filter").FilterType<Diver>()
          .Resolve(ctx =>
          {
              var filter = ctx.GetFilterExpression<Diver>("filter");
              var datasource = ctx.RequestServices!.GetRequiredService<IDivers>();
              if (filter != null)
                  return datasource.Divers.Where(filter.Compile());
              else
                  return null;
          });

        Field<NonNullGraphType<ListGraphType<NonNullGraphType<DiverGraphType>>>,IReadOnlyList<Diver>>("NonNullDiversReadOnly")
          .AddFilter("filter").FilterType<Diver>()
          .Resolve(ctx =>
          {
              var filter = ctx.GetFilterExpression<Diver>("filter");
              var datasource = ctx.RequestServices!.GetRequiredService<IDivers>();
              if (filter != null)
                  return datasource.Divers.Where(filter.Compile()).ToList();
              else
                  return null;
          });
    }
}
