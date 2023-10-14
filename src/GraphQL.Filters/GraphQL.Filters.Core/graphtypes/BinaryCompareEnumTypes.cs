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
        Add("equal" , (ConditionType condition,ParameterExpression arg)=> Binary(condition,arg,Expression.Equal));
        Add("greater" , (ConditionType condition,ParameterExpression arg)=> Binary(condition,arg,Expression.GreaterThan) );
        Add("greaterOrEqual" , (ConditionType condition,ParameterExpression arg)=> Binary(condition,arg,Expression.GreaterThanOrEqual) );
        Add("less" , (ConditionType condition,ParameterExpression arg)=> Binary(condition,arg,Expression.LessThan) );
        Add("lessOrEqual" , (ConditionType condition,ParameterExpression arg)=> Binary(condition,arg,Expression.LessThanOrEqual) );
    }

    static Expression Binary(ConditionType condition,ParameterExpression arg , Func<Expression,Expression,BinaryExpression> _factory) {
        var val = condition.Value;
        var eq = _factory.Invoke(condition.GetMemberExpression(arg) ?? throw new NullReferenceException(), val);
        return eq;
        }
}
