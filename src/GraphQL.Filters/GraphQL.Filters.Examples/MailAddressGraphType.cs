using System.Net.Mail;
using GraphQL.Types;
using GraphQLParser.AST;

namespace GraphQL.Filters.Examples;

public class MailAddressGraphType : StringGraphType
{
    public MailAddressGraphType()
    {
        Name = "MailAddress";
    }

    public override object? ParseLiteral(GraphQLValue value)
    {
        if (value == null) return null;
        if (value is GraphQLStringValue stringValue)
        {
            MailAddress? result;
            if (MailAddress.TryCreate((string)stringValue.Value, out result))
            {
                return result;
            }
        }
        return ThrowLiteralConversionError(value);
    }

    public override object? ParseValue(object? value)
    {
        if (value == null) return null;
        if (value is string stringValue)
        {
            MailAddress? result;
            if (MailAddress.TryCreate(stringValue, out result))
            {
                return result;
            }
        }
        return ThrowValueConversionError(value);
    }

    public override object? Serialize(object? value)
    {
        if(value == null) return null;
        if( value is MailAddress mailAddress)
        {
            return mailAddress.Address;
        }
        return ThrowSerializationError(value);
    }
}
