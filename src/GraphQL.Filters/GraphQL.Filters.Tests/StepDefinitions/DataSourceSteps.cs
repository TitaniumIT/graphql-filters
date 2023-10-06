using GraphQL.Filters.Tests.Drivers;
using GraphQL.Filters.Tests.Support;
using System.Collections;
using System.Reflection;
using TechTalk.SpecFlow.Assist;

namespace GraphQL.Filters.Tests.StepDefinitions
{
    [Binding]
    public class DataSourceSteps
    {
        private readonly DataDriver _dataDriver;
        public DataSourceSteps(DataDriver dataDriver) 
        {
            _dataDriver = dataDriver;
        }

        [Given("SimpleObject (.*) list")]
        public void ListOfSimpleObject(string listname,Table table)
        {
            _dataDriver.SimpleObjectLists[listname] = table.CreateSet<SimpleObject>();
        }
    }
}
