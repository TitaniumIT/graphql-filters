Feature: Nested Object filters

  Background:
    Given A Schema with QueryType as Query
    And Query has Field NestedObjects as List of NestedObjectType
    And Field NestedObjects has filtering of NestedObject
    And Field NestedObjects uses NestedObject filtered list
    When Create Schema

Scenario: Filter On nested simpletype
  Given Query operation FilterNestedDirectOnNestedSimples
  Given NestedObject list
    | StringMember |
    | Nested1      |
    | Nested2      |
  Given NestedObject Nested1 has Simples
    | StringMember | IntMember | DateTimeMember | DateOnlyMember | TimeOnlyMember | DecimalMember |
    | String 1     | 0         | 1/1/2001 00:00 | 1/1/2001       | 00:00          | 0.0           |
    | String 2     | 2         | 1/2/2001 00:00 | 1/2/2001       | 02:00          | 2.2           |
 Given NestedObject Nested2 has Simples
    | StringMember | IntMember | DateTimeMember | DateOnlyMember | TimeOnlyMember | DecimalMember |
    | String 3     | 0         | 1/1/2001 00:00 | 1/1/2001       | 00:00          | 0.0           |
    | String 4     | 2         | 1/2/2001 00:00 | 1/2/2001       | 02:00          | 2.2           |
  When Executed