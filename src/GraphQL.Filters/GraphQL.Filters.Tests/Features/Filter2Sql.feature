Feature: Filter to Sql

Scenario: Simple condition
    Given Query operation GetDiver2SqlSimple
    When Executed
    Then No errors
    And Data contains diver2Sql all
    | diver2Sql  |
    | D.[ID] = 1 |

Scenario: And condition
    Given Query operation GetDiver2SqlAnd
    When Executed
    Then No errors
    And Data contains diver2Sql all
    | diver2Sql                     |
    | (D.[ID] = 1) and (D.[ID] = 1) |

Scenario: Or condition
    Given Query operation GetDiver2SqlOr
    When Executed
    Then No errors
    And Data contains diver2Sql all
    | diver2Sql                    |
    | (D.[ID] = 1) or (D.[ID] = 1) |

Scenario: OrAnd condition
    Given Query operation GetDiver2SqlOrAnd
    When Executed
    Then No errors
    And Data contains diver2Sql all
    | diver2Sql                                       |
    | (D.[ID] = 1) or ((D.[ID] = 1) and (D.[ID] = 1)) |

Scenario: not condition
    Given Query operation GetDiver2SqlWithNot
    When Executed
    Then No errors
    And Data contains diver2Sql all
    | diver2Sql      |
    | NOT D.[ID] = 1 |

Scenario: Any construct
    Given Query operation GetDivers2SqlWithCoarseFilters
    When Executed
    Then No errors
    And Data contains diver2Sql all
    | diver2Sql              |
    | C.[Name] = 'OpenWater' |

Scenario: Any construct with And on Parent
    Given Query operation GetDivers2SqlWithCoarseFiltersAndDiver
    When Executed
    Then No errors
    And Data contains diver2Sql all
    | diver2Sql                                 |
    | (D.[ID] = 1) and (C.[Name] = 'OpenWater') |


Scenario: Any construct on autogerated mapping
    Given Query operation GetDivers2SqlWithDivesAt
    And Variables:
"""
        location: Twiske
"""
    When Executed
    Then No errors
    And Data contains diver2Sql all
    | diver2Sql                  |
    | (DIVE.[timestamp] = 1/1/2023) and (DIVE.[Location] = 'Twiske') |

