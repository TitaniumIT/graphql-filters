using FluentAssertions.Equivalency;
using GraphQL.Builders;
using GraphQL.Filters.Tests.Drivers;
using GraphQL.Filters.Tests.Support;
using GraphQL.Introspection;
using GraphQL.MicrosoftDI;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using nl.titaniumit.graphql.filters;
using nl.titaniumit.graphql.filters.models;
using NUnit.Framework;
using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow.Tracing;

namespace GraphQL.Filters.Tests.StepDefinitions;

[Binding]
public class SchemaSteps
{
    private readonly GraphQlDriver _driver;
    private readonly DataDriver _data;
    private readonly ScenarioContext _context;
    private readonly ISpecFlowOutputHelper _output;
    private Type? _queryType;
    private Dictionary<string, FieldBuilder<object?, object>>? _queryFields;

    public SchemaSteps(GraphQlDriver driver, ScenarioContext context, ISpecFlowOutputHelper output, DataDriver data)
    {
        _driver = driver;
        _context = context;
        _output = output;
        _data = data;
    }

    [Given("A Schema with (.*) as Query")]
    public void SetupSchemaTypes(string queryType)
    {
        _queryType = Type.GetType($"GraphQL.Filters.Tests.Support.{queryType}") ?? throw new NullReferenceException();
        _queryFields = new();
    }

    [Given("Query has Field (.*) as List of (.*)")]
    public void SetupListField(string _fieldname, string _listType)
    {
        Type _graphType = Type.GetType($"GraphQL.Filters.Tests.Support.{_listType}") ?? throw new NullReferenceException();
        var list = typeof(ListGraphType<>).MakeGenericType(_graphType);
        if (_queryFields != null)
        {
            _queryFields[_fieldname] = FieldBuilder<object?, object>.Create(list, _fieldname);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    [Given("Field (.*) has filtering of (.*)")]
    public void SetupFieldFilter(string _fieldname, string _filtertype)
    {
        if (_queryFields != null)
        {
            var type = typeof(FilterBuilder<object?, object>);
            var filterType = Type.GetType($"GraphQL.Filters.Tests.Support.{_filtertype}") ?? throw new NullReferenceException();
            var filterTypeMethod = type.GetMethod(nameof(FilterBuilder<object?, object>.FilterType))?.MakeGenericMethod(filterType) ?? throw new InvalidOperationException();
            var b = _queryFields[_fieldname].AddFilter("filter");

            filterTypeMethod.Invoke(b, new object[] { });
        }
        else
        {
            Assert.Fail();
        }
    }

    [Given("Field (.*) uses SimpleObject list (.*)")]
    [Given("Field (.*) uses SimpleObject filtered list (.*)")]
    public void SetupListResolversSimpleObject(string fieldname,string listname)
    {
        if (_queryFields != null)
        {
            bool useFiltering = _context.StepContext.StepInfo.Text.Contains("filtered list");
            var field = _queryFields[fieldname];
            field.Resolve()
            .Resolve(ctx =>
            {
                if (useFiltering)
                {
                    var filter = ctx.GetArgument<FilterType>("filter");
                    if (ctx.FieldDefinition.Arguments != null)
                    {
                        var expression = filter.CreateFilter<SimpleObject>();
                        _output.WriteLine(expression.ToString());
                        return _data.SimpleObjectLists[listname].Where(expression.Compile());
                    }
                    else
                    {
                        Assert.Fail();
                        return null;
                    }
                }
                else
                {
                    return _data.SimpleObjectLists[listname];
                }
            });
        }
        else
        {
            Assert.Fail();
        }
    }

    ISchema? _schema;

    [When("Create Schema")]
    public void CreateSchema()
    {
        if (_queryFields != null && _queryFields.Any() && _queryType != null)
        {
            _driver.Services.AddSingleton(srv => new TestSchema(new SelfActivatingServiceProvider(srv), _queryType));
            _driver.Services.AddSingleton(_queryType, srv => Activator.CreateInstance(_queryType, _queryFields.Values.ToList()) ?? throw new InvalidOperationException());
        }
        else
        {
            Assert.Fail();
        }


        _schema = _driver.Provider.GetRequiredService<TestSchema>();
        _output.WriteLine(_schema.Print());
    }

    [Then("Schema contains type (.*)")]
    public void SchemaContainsType(string typename)
    {
        if (_schema != null)
        {
            _schema.AllTypes.Should().Contain(type => type.Name == typename);
        }
    }

    [Then("Schema Enum (.*) contains (.*)")]
    public void SchemaEnumContainsType(string enumname,string member)
    {
        if (_schema != null)
        {
           if(  _schema.AllTypes.Single(type => type.Name == enumname) is EnumerationGraphType enumtype)
            {
                enumtype.Values.Should().Contain( e => e.Name == member);
            }
        }
    }

    [Then("Schema Enum (.*) doesnot contain (.*)")]
    public void SchemaEnumNotContainsType(string enumname, string member)
    {
        if (_schema != null)
        {
            if (_schema.AllTypes.Single(type => type.Name == enumname) is EnumerationGraphType enumtype)
            {
                enumtype.Values.Should().NotContain(e => e.Name == member);
            }
        }
    }
}
