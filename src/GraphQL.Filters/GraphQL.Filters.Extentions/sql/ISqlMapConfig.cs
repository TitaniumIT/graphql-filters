using System.Reflection;
using nl.titaniumit.graphql.filters.extentions.sql.ast;

namespace nl.titaniumit.graphql.filters.extentions;

public interface ISqlMapConfig
{
   internal SqlName GetSqlName(MemberInfo? fieldName);
}
