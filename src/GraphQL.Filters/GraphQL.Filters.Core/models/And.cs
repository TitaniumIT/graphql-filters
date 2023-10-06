using System;
using System.Linq;

namespace nl.titaniumit.graphql.filters.models
{
    internal record And(FilterType left, FilterType right);
}
