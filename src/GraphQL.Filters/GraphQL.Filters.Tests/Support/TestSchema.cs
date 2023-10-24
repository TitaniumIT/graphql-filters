using GraphQL.Builders;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using nl.titaniumit.graphql.filters;

namespace GraphQL.Filters.Tests.Support
{
    public class TestSchema : Schema
    {
        public TestSchema(IServiceProvider serviceProvider,Type queryType) : base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService(queryType) as IObjectGraphType ?? throw new NullReferenceException();

            RegisterTypeMapping(typeof(SimpleObject),typeof(SimpleObjectType));
        }
    }
    public class QueryType : ObjectGraphType
    {
        public QueryType(List<FieldBuilder<object?,object>> _builders)
        {
            foreach (var builder in _builders) 
            {
                AddField(builder.FieldType);
            }
        }
    }
    public record SimpleObject(string StringMember, int IntMember, DateTime DateTimeMember, DateOnly DateOnlyMember, TimeOnly TimeOnlyMember, decimal DecimalMember)
    {
        private readonly int HiddenField = 0;

        public string StringField= "";
    }

    public record NestedObject(string StringMember)
    {
        public List<SimpleObject> Simples { get;set;} = new List<SimpleObject>();
    }

    public class SimpleObjectType : AutoRegisteringObjectGraphType<SimpleObject>
    {
    }

    public class NestedObjectType : AutoRegisteringObjectGraphType<NestedObject>
    {
    }
    
}
