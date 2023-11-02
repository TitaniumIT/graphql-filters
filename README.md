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
supported 
* and
* or 
* not

## Collections
to filter collection members the any field on the filtertype is present
that contains per colletion field a member.

## Resolvers

# Setup

# Todo

* documentation
* more test cases
* custom filter functions
