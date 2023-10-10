using GraphQL.Filters.Tests.Drivers;
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
        }

        [Given("GraphQl query (.*)")]
        public void Given(string queryname)
        {

        }

    }
}