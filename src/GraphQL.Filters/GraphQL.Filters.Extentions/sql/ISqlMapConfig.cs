using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using nl.titaniumit.graphql.filters.extentions.sql.ast;

namespace nl.titaniumit.graphql.filters.extentions;

public interface ISqlMapConfig
{
   internal SqlName GetSqlName(MemberInfo? fieldName);

}

internal class SqlMapConfig : ISqlMapConfig
{
   internal Dictionary<Type, string> Tables { get; init; } = new();
   internal Dictionary<MemberInfo, string> Fields { get; init; } = new();
   SqlName ISqlMapConfig.GetSqlName(MemberInfo? fieldName) =>
       new SqlName(Fields[fieldName!],Tables[fieldName!.DeclaringType!]);
}

public static class SqlMapConfigExtentions
{

   public static ISqlMapConfig AddTableFor<T>(this ISqlMapConfig config, string? alias = null)
   {
      if (config is SqlMapConfig mapConfig)
      {
         Type t = typeof(T);
         if (alias == null)
         {
            alias = t.Name;
         }
         if (mapConfig.Tables.Values.Any(x => x == alias))
         {
            throw new InvalidOperationException($"Alias with name {alias} already exists");
         }
         mapConfig.Tables.Add(t,alias);
      }
      else
      {

      }
      return config;
   }

   public static ISqlMapConfig AddFieldFor<T>(this ISqlMapConfig config, MemberInfo memberInfo, string? alias = null)
   {
   if (config is SqlMapConfig mapConfig)
      {
         Type t = typeof(T);
         if (alias == null)
         {
            alias = memberInfo.Name;
         }
         mapConfig.Fields.Add(memberInfo,alias);
      }
      else
      {

      }
      return config;
   }

   public static ISqlMapConfig AddFieldFor<T>(this ISqlMapConfig config, Expression<Func<T,object?>> field, string? alias = null)
   {
      if ( field is LambdaExpression lambdaExpression)
      {
         if ( lambdaExpression.Body is MemberExpression memberExpression)
         {
            config.AddFieldFor<T>(memberExpression.Member,alias);
            return config;
         }
         if ( lambdaExpression.Body is UnaryExpression convert  
                  && convert.NodeType == ExpressionType.Convert
                  && convert.Operand is MemberExpression converted)
         {
            config.AddFieldFor<T>(converted.Member ,alias);
            return config;
         }
      }
     throw new NotSupportedException($"Unsupported expression {field}");
   }
}


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