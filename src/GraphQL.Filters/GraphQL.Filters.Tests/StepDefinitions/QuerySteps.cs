using GraphQL.Filters.Tests.Drivers;
using GraphQL.Filters.Tests.Support;
using GraphQL.SystemTextJson;
using GraphQL.Validation;
using GraphQLParser;
using GraphQLParser.AST;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Dynamic;
using System.Security;
using System.Text.Json;
using System.Text.Json.Serialization;
using TechTalk.SpecFlow.Assist;
using TechTalk.SpecFlow.Infrastructure;

namespace GraphQL.Filters.Tests.StepDefinitions
{
    [Binding]
    public class QuerySteps
    {
        private readonly GraphQlDriver _driver;
        private readonly DataDriver _data;
        private readonly ScenarioContext _context;
        private readonly ISpecFlowOutputHelper _output;

        static private readonly Dictionary<string, GraphQLParser.AST.GraphQLOperationDefinition> _queries = new();

        public QuerySteps(GraphQlDriver driver, ScenarioContext context, ISpecFlowOutputHelper output, DataDriver data)
        {
            _driver = driver;
            _context = context;
            _output = output;
            _data = data;
        }

        [BeforeFeature]
        static public void BeforeFeature(FeatureContext featureContext)
        {
            _queries.Clear();
            var files = Directory.GetFiles("Features", "*.graphql");
            foreach (var file in files)
            {
                var astTree = Parser.Parse(new ROM(File.ReadAllText(file).AsMemory()));
                foreach (GraphQLOperationDefinition def in astTree.Definitions)
                {
                    if (def != null && def.Operation == OperationType.Query)
                    {
                        _queries.Add(def.Name?.StringValue ?? throw new NullReferenceException(), def);
                    }
                }
            }
        }

        string _currentQuery;

        [Given("Query operation (.*)")]
        public void Given(string queryname)
        {
            _queries.Keys.Should().Contain(queryname);
            _currentQuery = queryname;
            _executionResult = null;
        }

        Inputs? _variables = null;

        [Given("Variables:")]
        public void Variables(string _yaml)
        {
            var dict = new SharpYaml.Serialization.Serializer().Deserialize<Dictionary<object, object?>>(_yaml) ?? throw new NullReferenceException();
            _variables =
                 new Inputs(
                    ConvertTo(dict)
                 );
        }

        IDictionary<string, object?> ConvertTo(IDictionary<object, object?> dict)
        {
            return dict.ToDictionary(k => k.Key.ToString()!, v =>
            {
                if (v.Value is IDictionary<object, object?> childDict)
                {
                    return ConvertTo(childDict);
                }
                return v.Value;
            });
        }

        ExecutionResult? _executionResult = null;

        [When("Executed")]
        public async Task ExecuteQuery()
        {
            var provider = _driver.Provider;
            var schema = provider.GetRequiredService<TestSchema>();
            var executer = provider.GetRequiredService<IDocumentExecuter>();
            var rules = provider.GetService<IEnumerable<IValidationRule>>();

            _executionResult = await executer.ExecuteAsync(options =>
            {
                options.Schema = schema;
                options.Document = new GraphQLDocument(new List<ASTNode>() { _queries[_currentQuery] });
                options.OperationName = _currentQuery;
                options.Variables = _variables != null ? _variables : null;
                options.ValidationRules = Validation.DocumentValidator.CoreRules.Concat(rules??Array.Empty<IValidationRule>());
            });

            _output.WriteLine(new GraphQLSerializer().Serialize(_executionResult));
        }

        [Then("No errors")]
        public void NoErrors()
        {
            _executionResult.Should().NotBeNull();
            if (_executionResult != null)
            {
                _executionResult.Errors.Should().BeNullOrEmpty();
            }
        }

        [Then("Should have errors")]
        public void HasEDrros()
        {
            _executionResult.Should().NotBeNull();
            if (_executionResult != null)
            {
                _executionResult.Errors.Should().NotBeEmpty();
            }
        }

        [Then("Data contains (.*)")]
        public void DataContains(string nestedProperty, Table table)
        {
            var expected = table.CreateDynamicSet();

            string json = new GraphQLSerializer().Serialize(_executionResult);

            var document = JsonDocument.Parse(json);

            var dataNode = document.RootElement.GetProperty("data").GetProperty(nestedProperty);

            var data = JsonSerializer.Deserialize<IEnumerable<Dictionary<string,JsonElement>>>(dataNode);

           var dictionaries= data.Select( x => x.ToDictionary( k => k.Key , k => {
               return k.Value.ValueKind switch
               {
                   JsonValueKind.String => k.Value.GetString() as object,
                   JsonValueKind.Number => k.Value.GetInt32() as object,
                   JsonValueKind.Null => null as object,
               };
             }));

            dictionaries.Should().BeEquivalentTo(expected);
        }
    }
}