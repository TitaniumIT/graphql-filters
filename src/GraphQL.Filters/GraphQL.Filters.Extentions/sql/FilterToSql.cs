using GraphQL;
using Microsoft.Extensions.DependencyInjection;
using nl.titaniumit.graphql.filters.models;

namespace nl.titaniumit.graphql.filters.extentions.sql;

static public class FilterToSqlExtentions
{
    public static string GetWhere(this IResolveFieldContext resolveFieldContext , string filtername)
    {

        var visitor =  new FilterVisitorSql(resolveFieldContext
                ,resolveFieldContext.RequestServices!.GetRequiredService<ISqlMapConfig>());

        visitor.Visit( resolveFieldContext.GetArgument<FilterType>(filtername));

        return visitor.GetWhere();
    }
}
