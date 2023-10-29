using System.Net.Mail;
using GraphQL.Filters.Examples;
using GraphQL.Filters.Tests.Drivers;
using TechTalk.SpecFlow.Assist;
using TechTalk.SpecFlow.Assist.ValueRetrievers;

namespace GraphQL.Filters.Tests.StepDefinitions;

[Binding]
public class DataSourceSteps
{
    private readonly DataDriver _dataDriver;
    public DataSourceSteps(DataDriver dataDriver)
    {
        _dataDriver = dataDriver;
    }


    [Given("the following divers:")]
    public void DiversSetup(Table table)
    {
        _dataDriver.Divers = table.CreateSet<Diver>().Concat(_dataDriver.Divers);
    }


    [Given("the following coarses for diver (.*)")]
    public void DiversSetup(int id,Table table)
    {
        _dataDriver.Divers.Single( x => x.Id == id )
            .Courses = table.CreateSet<Course>().ToList();
    }


}
