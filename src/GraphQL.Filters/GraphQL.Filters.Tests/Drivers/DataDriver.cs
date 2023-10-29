using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;
using GraphQL.Filters.Examples;
using TechTalk.SpecFlow.Assist;
using TechTalk.SpecFlow.Assist.ValueRetrievers;

namespace GraphQL.Filters.Tests.Drivers;

public class DataDriver : IDivers
{
    public IEnumerable<Diver> Divers { get; set; } = new List<Diver>();

    static DataDriver()
    {
        Service.Instance.ValueRetrievers.Register(new MailAddressValueConvertor());
        Service.Instance.ValueRetrievers.Register(new NullValueRetriever("@null"));
        Service.Instance.ValueRetrievers.Register(new ObjectNullRetriever("@null"));
    }
    private class MailAddressValueConvertor : IValueRetriever
    {
        public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            return typeof(Diver) == targetType && propertyType == typeof(MailAddress) && keyValuePair.Key == nameof(Diver.Email);
        }

        public object Retrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            return new MailAddress(keyValuePair.Value);
        }
    }

    private class ObjectNullRetriever : IValueRetriever

    {
        string _nullvalue;
        public ObjectNullRetriever(string nullValue)
        {
            _nullvalue = nullValue;
        }
        public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            return typeof(object) == propertyType && keyValuePair.Value == _nullvalue;
        }

        public object Retrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            if ( keyValuePair.Value == _nullvalue)
            return null!;
            else return keyValuePair.Value;
        }
    }
}
