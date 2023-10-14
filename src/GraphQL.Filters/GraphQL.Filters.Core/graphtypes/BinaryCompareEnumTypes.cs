using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using nl.titaniumit.graphql.filters.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace nl.titaniumit.graphql.filters.graphtypes;

public class BinaryCompareEnumTypes : EnumerationGraphType
{
    public BinaryCompareEnumTypes()
    {
        Add("equal" , Equal );
        Add("greater" , Greater );
    }

    Expression Equal(ConditionType condition,ParameterExpression arg) {
        var val = condition.Value;
        var eq = Expression.Equal(condition.GetMemberExpression(arg) ?? throw new NullReferenceException(), val);
        return eq;
        }

       Expression Greater(ConditionType condition,ParameterExpression arg) {
        var val = condition.Value;
        var eq = Expression.GreaterThan(condition.GetMemberExpression(arg) ?? throw new NullReferenceException(), val);
        return eq;
        }
}
