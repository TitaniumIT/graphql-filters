using GraphQL.Filters.Examples;
using GraphQL.Filters.Tests.Drivers;
using GraphQL.MicrosoftDI;
using Microsoft.Extensions.DependencyInjection;
using TechTalk.SpecFlow.Infrastructure;

namespace GraphQL.Filters.Tests.StepDefinitions;

[Binding]
public class SchemaSteps
{
    private readonly GraphQlDriver _driver;
    private readonly DataDriver _data;
    private readonly ScenarioContext _context;
    private readonly ISpecFlowOutputHelper _output;

    public SchemaSteps(GraphQlDriver driver, ScenarioContext context, ISpecFlowOutputHelper output, DataDriver data)
    {
        _driver = driver;
        _context = context;
        _output = output;
        _data = data;
    }

    [BeforeScenario]
    public void LoadSchema()
    {
          _driver.Services.AddSingleton(srv => new DivingSchema(new SelfActivatingServiceProvider(srv)));
          _driver.Services.AddSingleton<IDivers>((srv) =>_data);
          _driver.Services.AddSingleton<IDives>((srv) =>_data);
    }

    [Then("Print schema")]
    public void PrintSchema()
    {
        var provider = _driver.Provider;
        var schema = provider.GetRequiredService<Examples.DivingSchema>();
        var sdl = schema.Print();
        File.WriteAllText("../../../Support/DivingSchema.graphql",sdl);
        _output.WriteLine(sdl);
    }
}
