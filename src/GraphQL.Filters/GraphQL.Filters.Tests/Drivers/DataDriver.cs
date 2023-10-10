using GraphQL.Filters.Tests.Support;
using System;
using System.Linq;

namespace GraphQL.Filters.Tests.Drivers
{
    public class DataDriver
    {
        public Dictionary<string, IEnumerable<SimpleObject>> SimpleObjectLists = new Dictionary<string, IEnumerable<SimpleObject>>();
    }
}
