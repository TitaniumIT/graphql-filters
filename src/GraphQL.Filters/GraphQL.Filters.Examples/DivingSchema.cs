using System.Net.Mail;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL.Filters.Examples;

public class DivingSchema : Schema
{
    public DivingSchema(IServiceProvider serviceProvider):base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<DivingQueryGraphType>();

        RegisterTypeMapping(typeof(MailAddress),typeof(MailAddressGraphType));
        RegisterTypeMapping(typeof(Course),typeof(CourseGraphType));
         RegisterTypeMapping(typeof(Diver),typeof(DiverGraphType));
    }
}
