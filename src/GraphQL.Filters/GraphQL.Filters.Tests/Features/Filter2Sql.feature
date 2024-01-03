Feature: Filter to Sql

Scenario: Simple condition
Given Query operation GetDiver2SqlSimple
When Executed
Then No errors
And Data contains diver2Sql all
| diver2Sql |
| D.[ID] = 1|

Scenario: And condition
Given Query operation GetDiver2SqlAnd
When Executed
Then No errors
And Data contains diver2Sql all
| diver2Sql |
| (D.[ID] = 1) and (D.[ID] = 1)|
