﻿using GraphQL.Types;
using System;
using System.Linq;

namespace nl.titaniumit.graphql.filters.graphtypes
{
    public class ValueScalar : ScalarGraphType
    {
        public override object? ParseValue(object? value)
        {
            return value;
        }
    }
}
