using GraphQL.Execution;
using GraphQL.Filters.Tests.Drivers;
using GraphQL.SystemTextJson;
using GraphQL.Validation;
using GraphQLParser;
using GraphQLParser.AST;
using GraphQLParser.Visitors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Dynamic;
using System.Text.Json;
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
        static public void BeforeFeature(FeatureContext featureContext, ISpecFlowOutputHelper output)
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

        string _currentQuery = "";

        [Given("Query operation (.*)")]
        public async Task Given(string queryname)
        {
            _queries.Keys.Should().Contain(queryname);
            _currentQuery = queryname;

            OperationWriter writer = new();
            await writer.Write(_output, _queries[_currentQuery]);
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
            var schema = provider.GetRequiredService<Examples.DivingSchema>();
            var executer = provider.GetRequiredService<IDocumentExecuter>();
            var rules = provider.GetService<IEnumerable<IValidationRule>>();

            using var scope = provider.CreateScope();

            _executionResult = await executer.ExecuteAsync(options =>
            {
                options.Schema = schema;
                options.Document = new GraphQLDocument(new List<ASTNode>() { _queries[_currentQuery] });
                options.OperationName = _currentQuery;
                options.Variables = _variables != null ? _variables : null;
                options.ValidationRules = Validation.DocumentValidator.CoreRules.Concat(rules ?? Array.Empty<IValidationRule>());
                options.RequestServices = scope.ServiceProvider;
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

       [Then("Data is not empty")]
        public void DataNotEmpty()
        {
            _executionResult.Should().NotBeNull();
            if (_executionResult != null)
            {
                _executionResult.Data.Should().NotBeNull();
            }
        }

        [Then("Should have errors")]
        public void HasErros()
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
            var expected = table.Rows.Select(row =>
            {
                var instance = new ExpandoObject();
                var dict = instance as IDictionary<string, object?>;
                foreach (var keyval in row.Select(col =>
                        {
                            var valueRetriever = GetValueRetrieverFor(col, typeof(ExpandoObject), typeof(object));
                            object? value = col.Value;
                            if (valueRetriever != null)
                            {
                                value = valueRetriever.Retrieve(col, typeof(ExpandoObject), typeof(object));
                            }
                            return new KeyValuePair<string, object?>(col.Key, value);
                        }))
                {
                    dict.Add(keyval.Key, keyval.Value);
                }
                return instance;
            }).Cast<dynamic>().ToList();

            string json = new GraphQLSerializer().Serialize(_executionResult);

            var document = JsonDocument.Parse(json);

            var dataNode = document.RootElement.GetProperty("data").GetProperty(nestedProperty);

            if (dataNode.ValueKind == JsonValueKind.Array)
            {
                IEnumerable<Dictionary<string, object?>> dictionaries = GetArray(dataNode);

                expected.Should().BeEquivalentTo(dictionaries);
            }
            if (dataNode.ValueKind == JsonValueKind.Object)
            {
                var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(dataNode);
                if (data == null) throw new InvalidOperationException();
                var dictionaries = data.ToDictionary(k => k.Key, k =>
                {
                    return ProcessElement(k);
                });

                Dictionary<string, object?> exp = new(expected.First());
                dictionaries.Should().BeEquivalentTo(exp, config =>
                {
                    config.WithAutoConversion();
                    return config;
                });
            }

        }

        private static IEnumerable<Dictionary<string, object?>> GetArray(JsonElement dataNode)
        {
            var data = JsonSerializer.Deserialize<IEnumerable<Dictionary<string, JsonElement>>>(dataNode);

            var dictionaries = data!.Select(x => x.ToDictionary(k => k.Key, k =>
            {
                return ProcessElement(k);
            }));
            return dictionaries;
        }

        private static object? ProcessElement(KeyValuePair<string, JsonElement> k)
        {
            return k.Value.ValueKind switch
            {
                JsonValueKind.String => k.Value.GetString() as object,
                JsonValueKind.Number => k.Value.GetInt32() as object,
                JsonValueKind.Array => GetArray(k.Value) as object,
                JsonValueKind.Null => null as object,
                _ => throw new InvalidOperationException()
            };
        }

        public IValueRetriever? GetValueRetrieverFor(KeyValuePair<string, string> row, Type targetType, Type propertyType)
        {

            foreach (IValueRetriever valueRetriever in Service.Instance.ValueRetrievers)
            {
                if (valueRetriever.CanRetrieve(row, targetType, propertyType))
                {
                    return valueRetriever;
                }
            }

            return null;
        }
    }
}