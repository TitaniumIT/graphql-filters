Feature: Schema generation

Scenario: Schema with SimpleObject filters
should contain SimpleObject filter types and field
enums should contain public fields

Given A Schema with QueryType as Query
And Query has Field SimpleObjects as List of SimpleObjectType
And Field SimpleObjects has filtering of SimpleObject
When Create Schema
Then Schema contains type FilterGraphTypeSimpleObject
Then Schema contains type AndGraphTypeSimpleObject
Then Schema contains type ConditionGraphTypeSimpleObject
Then Schema Enum FieldEnumerationGraphTypeSimpleObject contains StringMember

