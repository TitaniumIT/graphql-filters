# Introduction

An extention on GraphQL.net to add filtering on clr types

# Example graphql filters

Filters are based on a clrType/interface

Using AddFilter as argument.  The graphql type for the filters are automaticly added

* FilterGraphType<typename>
* And/Or/Not types
* Condition type
* Any

## Simple filters 

```graphql
condition:{
    fieldName:Id
    operator: equal
    value: 1
}
```

```graphql
condition:{
    fieldName: DateField
    operator: equal
    value: "10-10-2023" 
}
```
### Operators on scalars
supported operators on scalartypes
* equal
* greater
* greaterOrEqual
* less
* lessOrEqual
* notEqual

 
## Booleans
supported 
* and
* or 
* not

## Collections
to filter collection members the any field on the filtertype is present
that contains per colletion field a member.

## Resolvers

Using filters in a Resolver.  

```csharp
Field<DiverGraphType>("Diver")
    .AddFilter("filter").FilterType<Diver>()
    .Resolve(ctx =>
    {
        var filter = ctx.GetFilterExpression<Diver>("filter");
        var datasource = ctx.RequestServices!.GetRequiredService<IDivers>();
        if (filter != null)
            return datasource.Divers.SingleOrDefault(filter.Compile());
        else
            return null;
    });
```

When getting a filter on parent for a child
```csharp
 Field<ListGraphType<DiveGraphType>>("Dives")
            .Resolve( ctx => {
              var datasource = ctx.RequestServices!.GetRequiredService<IDives>();
              var expression = ctx.GetSubFilterExpression<Dive>();
              if( expression != null){
                return datasource.Dives.Where( d => d.Diver?.Id == ctx.Source.Id).Where(expression.Compile());
              } else{
                return datasource.Dives.Where( d => d.Diver?.Id == ctx.Source.Id);
              }
            });
```

# query examples

```graphql
query GetDiverFixedFilter {
    diver(filter:{
        condition:{
            fieldName:id
            operator: equal
            value: 1
        }
    }){
        name
        email
        id
    }
}

query GetDiverByEmail {
    diver(filter:{
        condition:{
            fieldName: email
            operator: equal
            value: "john@divers.down"
        }
    }){
        name
        email
        id
    }
}

query GetDiversWithCoarseFilters {
    divers(filter:{
        any:{
            courses:{
                condition:{
                    fieldName: name
                    operator:equal
                    value: "OpenWater"
                }
            }
        }
    }){
        name
        email
        id
    }
}

query GetDiverFilterById($id: ValueScalar!) {
    diver(filter:{
        condition:{
            fieldName:id
            operator: equal
            value: $id
        }
    }){
        id
    }
}

query GetDiversWithDivesAt($location:ValueScalar!){
    divers(filter: {
        any:{
            dives:{
                condition:{
                    fieldName:location
                    operator:equal
                    value: $location
                }
            }
        }
    }){
        name
        dives{
            start
            end
        }
    }
}
```

# Setup



# Todo

* documentation
* more test cases
* custom filter functions
* configuration options like, casing.