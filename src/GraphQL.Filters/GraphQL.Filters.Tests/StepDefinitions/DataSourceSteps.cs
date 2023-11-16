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
    public void CoarsesSetup(int id,Table table)
    {
        _dataDriver.Divers.Single( x => x.Id == id )
            .Courses = table.CreateSet<Course>().ToList();
    }

    [Given("the following dives for diver (.*)")]
    public void DivesSetup(int id,Table table)
    {
        var diver = _dataDriver.Divers.Single( x => x.Id == id );
    
       _dataDriver.Dives = table.CreateSet<Dive>().Select( d=>{ d.Diver=diver; return d; }).ToList().Concat(_dataDriver.Dives);
    }


}
