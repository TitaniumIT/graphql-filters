using GraphQL.Filters.Tests.Support;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace GraphQL.Filters.Tests.Drivers
{
    public class GraphQlDriver
    {
        public IServiceCollection Services { get; }

        public GraphQlDriver()
        {
            Services = new ServiceCollection();

            Services.AddGraphQL(builder =>
            {
                builder.AddSystemTextJson();
                builder.AddErrorInfoProvider(a =>
                {
                    a.ExposeData = true;
                    a.ExposeExceptionDetails = true;
                });
                builder.ConfigureExecutionOptions(o =>
                {
                    o.ThrowOnUnhandledException = true;
                });
            });
        }
    }

    public class DataDriver
    {
        public Dictionary<string, IEnumerable<SimpleObject>> SimpleObjectLists = new Dictionary<string, IEnumerable<SimpleObject>>();
    }
}
