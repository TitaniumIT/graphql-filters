Feature: Validating Filter types

Scenario Outline: Filter input type should be present
    Given Query operation FilterType
    Given Variables:
            """
            name: <name>
            """
    When Executed
    Then No errors
    Then Data contains __type
            | name   | description   |
            | <name> | <description> |
    Examples:
            | name                    | description                                                        |
            | FilterGraphTypeDiver    | only use one of the fields and leave the rest empty. Don't combine |
            | ConditionGraphTypeDiver | valid combinations are fieldname,operator,value                    |
            | AndGraphTypeDiver       | @null                                                              |
            | OrGraphTypeDiver        | @null                                                              |
            | NotGraphTypeDiver       | @null                                                              |
            | AnyGraphTypeDiver       | @null                                                              |
