using System.Reflection;
using nl.titaniumit.graphql.filters.extentions.sql.ast;

namespace nl.titaniumit.graphql.filters.extentions;

internal class SqlMapConfig : ISqlMapConfig
{
   internal Dictionary<Type, string> Tables { get; init; } = new();
   internal Dictionary<MemberInfo, string> Fields { get; init; } = new();
   SqlName ISqlMapConfig.GetSqlName(MemberInfo? fieldName) =>
       new SqlName(Fields[fieldName!],Tables[fieldName!.DeclaringType!]);
       
}
