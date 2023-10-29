using Microsoft.Extensions.DependencyInjection;
using nl.titaniumit.graphql.filters;

namespace GraphQL.Filters.Tests.Drivers
{
    public class GraphQlDriver
    {
        public IServiceCollection Services { get; }

        public GraphQlDriver()
        {
            Services = new ServiceCollection();

            Services.AddGraphQLFilters();
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

        IServiceProvider? _provider;
        public IServiceProvider Provider
        {
            get
            {
                if (_provider == null)
                {
                    _provider = Services.BuildServiceProvider(true);
                }
                return _provider;
            }
        }
    }
}

