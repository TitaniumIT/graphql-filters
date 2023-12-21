using System.Linq.Expressions;
using System.Security.Cryptography;

namespace nl.titaniumit.graphql.filters;

public interface IFieldConditionDefinition
{
    public string FieldName {get;}
}
