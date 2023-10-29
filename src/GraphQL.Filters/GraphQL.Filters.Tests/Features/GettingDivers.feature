Feature: Getting divers using filters

Background: Divers
Given the following divers:
| Name | Email            | Id |
| John | john@divers.down | 1  |
| Eli  | eli@divers.down  | 2  |

Scenario: Getting single diver by Id fixed
Given Query operation GetDiverFixedFilter
When Executed
Then No errors
Then Data contains diver
| name | email            | id |
| John | john@divers.down | 1  |

Scenario Outline: Getting single diver by id variable

Given Query operation GetDiverFilterById
Given Variables:
"""
id: <id>
"""
When Executed
Then No errors
Then Data contains diver
| id   |
| <id> |

Examples:
| id |
| 1  |
| 2  |

Scenario: Getting Open water divers
Given the following coarses for diver 1
| Name      | Started  | Finished |
| OpenWater | 1-1-2023 | @null    |
Given Query operation GetDiversWithCoarseFilters
When Executed
Then No errors

Scenario: Getting single diver by email
Filtering on custom scalars
Given Query operation GetDiverByEmail
When Executed
Then No errors
Then Data contains diver
| name | email            | id |
| John | john@divers.down | 1  |
