using System.Linq.Expressions;
using GraphQL;
using nl.titaniumit.graphql.filters.models;

namespace nl.titaniumit.graphql.filters;

internal record BinaryCompareType( ExpressionType expressionType, 
 Func<ConditionType, Expression, IResolveFieldContext, Expression>? expression);