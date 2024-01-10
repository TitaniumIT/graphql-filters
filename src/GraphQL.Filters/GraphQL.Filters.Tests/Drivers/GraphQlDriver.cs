using GraphQL.Filters.Examples;
using Microsoft.Extensions.DependencyInjection;
using nl.titaniumit.graphql.filters;
using nl.titaniumit.graphql.filters.extentions;


namespace GraphQL.Filters.Tests.Drivers
{
    public class GraphQlDriver
    {
        public IServiceCollection Services { get; }

        public GraphQlDriver()
        {
            Services = new ServiceCollection();

            Services.AddSqlMap((config) =>{
                
                config.AddTableFor<Diver>("D");
                config.AddFieldFor<Diver>( x => x.Id , "ID");
                config.AddTableFor<Course>("C");
                config.AddFieldFor<Course>( x => x.Name , "Name");
                config.AddTableFor<Dive>("DIVE",true);
                config.AddFieldFor<Dive>( x => x.On , "timestamp");
            });

            Services.AddGraphQLFilters();
            Services.AddGraphQL(builder =>
            {
                builder.AddSystemTextJson();
                builder.AddDataLoader();
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

