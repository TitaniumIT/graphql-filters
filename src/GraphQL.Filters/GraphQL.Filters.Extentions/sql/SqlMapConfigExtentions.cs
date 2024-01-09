using System.Linq.Expressions;
using System.Reflection;

namespace nl.titaniumit.graphql.filters.extentions;

public static class SqlMapConfigExtentions
{

   public static ISqlMapConfig AddTableFor<T>(this ISqlMapConfig config, string? alias = null)
   {
      if (config is SqlMapConfig mapConfig)
      {
         Type t = typeof(T);
         alias ??= t.Name;
         if (mapConfig.Tables.Values.Any(x => x == alias))
         {
            throw new InvalidOperationException($"Alias with name {alias} already exists");
         }
         mapConfig.Tables.Add(t,alias);
      }
      return config;
   }

   public static ISqlMapConfig AddFieldFor<T>(this ISqlMapConfig config, MemberInfo memberInfo, string? alias = null)
   {
   if (config is SqlMapConfig mapConfig)
      {
         Type t = typeof(T);
         alias ??= memberInfo.Name;
         mapConfig.Fields.Add(memberInfo,alias);
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
