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
    }

    public class SimpleObjectType : AutoRegisteringObjectGraphType<SimpleObject>
    {
    }
}
