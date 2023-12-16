Feature: Validating Filter types

Scenario Outline: Filter input type should be present
        Given Query operation FilterType
        Given Variables:
"""
            name: <name>
"""
        When Executed
        Then No errors
        Then Data contains __type all
        | name   | description   |
        | <name> | <description> |
Examples:
        | name                    | description |
        | FilterGraphTypeDiver    | @null       |
        | ConditionGraphTypeDiver | @null       |
        | AndGraphTypeDiver       | @null       |
        | OrGraphTypeDiver        | @null       |
        | NotGraphTypeDiver       | @null       |
        | AnyGraphTypeDiver       | @null       |

Scenario Outline: Filed enum should
        Given Query operation FieldEnum
        Given Variables:
"""
            name: <name>
"""
        When Executed
        Then No errors

Examples:
        | name                           |
        | FieldEnumerationGraphTypeDiver |
