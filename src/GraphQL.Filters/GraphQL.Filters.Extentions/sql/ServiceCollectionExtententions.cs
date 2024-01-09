using Microsoft.Extensions.DependencyInjection;

namespace nl.titaniumit.graphql.filters.extentions;

public static class ServiceCollectionExtententions
{
   public static IServiceCollection AddSqlMap(this IServiceCollection serviceCollection, Action<ISqlMapConfig> build)
   {
      serviceCollection.AddSingleton<ISqlMapConfig>(
         (srv)=> {
            var config = new SqlMapConfig();
            build(config);
            return config;
         }
      );
      return serviceCollection;
   }
}