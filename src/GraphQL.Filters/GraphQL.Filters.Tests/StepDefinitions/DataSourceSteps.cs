using GraphQL.Filters.Tests.Drivers;
using GraphQL.Filters.Tests.Support;
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

        [Given("SimpleObject list")]
        public void ListOfSimpleObject(Table table)
        {
            _dataDriver.SimpleObjectLists = table.CreateSet<SimpleObject>();
        }

        [Given("NestedObject list")]
        public void ListOfNestedObjects(Table table)
        {
            _dataDriver.NestedObjects = table.CreateSet<NestedObject>();
        }

        [Given("NestedObject (.*) has Simples")]
        public void ListOfNestedObjectsSimpleObjeccts(string nestedtObject,Table table)
        {
            _dataDriver.NestedObjects.Single( x => x.StringMember == nestedtObject).Simples = table.CreateSet<SimpleObject>().ToList();
        }
    }
}
