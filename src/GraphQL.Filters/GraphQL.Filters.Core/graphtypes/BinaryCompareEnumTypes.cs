using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using nl.titaniumit.graphql.filters.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace nl.titaniumit.graphql.filters.graphtypes
{
    public class BinaryCompareEnumTypes : EnumerationGraphType<BinaryCompareTypes>
    {
        public BinaryCompareEnumTypes()
        {
        }
    }
}
