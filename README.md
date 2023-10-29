# Introduction

An extention on GraphQL.net to add filtering on clr types

# Example graphql filters

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

## Collections

## Resolvers

# Setup

