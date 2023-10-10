Feature: Filtering Simpleobject lists


Background:
Given A Schema with QueryType as Query
And Query has Field SimpleObjects as List of SimpleObjectType
And Field SimpleObjects has filtering of SimpleObject


Scenario: inline filter on equals on strings
And Field SimpleObjects uses list stringlist
When Create Schema
Given SimpleObject stringlist list
| StringMember | IntMember | DateTimeMember | DateOnlyMember | TimeOnlyMember | DecimalMember |
| String 1     | 0         | 1/1/2001 00:00 | 1/1/2001       | 00:00          | 0.0           |
| String 2     | 2         | 1/2/2001 00:00 | 1/2/2001       | 02:00          | 2.2           |
Given GraphQl query FilterSimpleObjectDirectEquals
