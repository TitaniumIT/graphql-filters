using GraphQL.Filters.Tests.Drivers;
using GraphQL.Filters.Tests.Support;
using GraphQL.SystemTextJson;
using GraphQLParser;
using GraphQLParser.AST;
using Microsoft.Extensions.DependencyInjection;
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
            var files = Directory.GetFiles("Features","*.graphql");
            foreach (var file in files)
            {
                var astTree = Parser.Parse(new ROM(File.ReadAllText(file).AsMemory()));
                foreach (GraphQLOperationDefinition def in astTree.Definitions)
                {
                    if ( def != null && def.Operation == OperationType.Query)
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
        }

        [Given("Variables:")]
        public void Variables(Table table)
        {
            var dict = new Dictionary<string, object>();

            foreach( var row in table.Rows)
            {
               var value = JsonSerializer.Deserialize<Dictionary<string, object>>(row.GetString("Value"));
                dict.Add( row.GetString("Name") , value! );
            }
        }

        [When("Execute")]         
        public async Task ExecuteQuery()
        {
            var provider = _driver.Provider;
            var schema = provider.GetRequiredService<TestSchema>();
            var executer = provider.GetRequiredService<IDocumentExecuter>();

            var result= await executer.ExecuteAsync(options =>
            {
                options.Schema = schema;
                options.Document = new GraphQLDocument(new List<ASTNode>() { _queries[_currentQuery] });
                options.OperationName = _currentQuery;
            });

            _output.WriteLine(new GraphQLSerializer().Serialize(result));
        }
    }
}