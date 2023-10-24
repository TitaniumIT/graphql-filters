using GraphQL.Filters.Tests.Support;
using System;
using System.Linq;

namespace GraphQL.Filters.Tests.Drivers
{
    public class DataDriver
    {
        public IEnumerable<SimpleObject> SimpleObjectLists = new List<SimpleObject>();
        public IEnumerable<NestedObject> NestedObjects = new List<NestedObject>();
    }
}
