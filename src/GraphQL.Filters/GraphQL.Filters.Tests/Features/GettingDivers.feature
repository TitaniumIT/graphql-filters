Feature: Getting divers using filters

Background: Divers
    Given the following divers:
        | Name  | Email            | Id | Bio      | BirthDate  |
        | John  | john@divers.down | 1  | My       | 1980-01-01 |
        | Eli   | eli@divers.down  | 2  | @null    | 1970-01-01 |
        | Harry | @null            | 3  | Some bio | @null      |

Scenario: Getting single diver by Id fixed
    Given Query operation GetDiverFixedFilter
    When Executed
    Then No errors
    Then Data contains diver all
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
    Then Data contains diver all
        | id   |
        | <id> |

    Examples:
        | id |
        | 1  |
        | 2  |

Scenario: Get divers using ands
    Given Query operation GetDiverFilterByAnds
    Given Variables:
        """
        id: 1
        """
    When Executed
    Then No errors
    Then Data contains diver all
        | id | name |
        | 1  | John |

Scenario: Getting Open water divers
    Given the following coarses for diver 1
        | Name      | Started  | Finished |
        | OpenWater | 1-1-2023 | @null    |
    Given Query operation GetDiversWithCoarseFilters
    When Executed
    Then No errors

Scenario: Getting Open water divers inprogress
    Given the following coarses for diver 1
        | Name      | Started  | Finished | CourseResult |
        | OpenWater | 1-1-2023 | @null    | InProgress   |
    Given Query operation GetDiversWithCoarseFiltersEnum
    When Executed
    Then No errors

Scenario: Getting single diver by email
    Filtering on custom scalars
    Given Query operation GetDiverByEmail
    When Executed
    Then No errors
    Then Data contains diver all
        | name | email            | id |
        | John | john@divers.down | 1  |

Scenario: Getting single diver with email as required field but not present
    Filtering requirements check should produce an error if not present
    Given Query operation GetDiverNotByEmailButRequired
    When Executed
    Then Should have errors
    And Errors are:
        | Message                               |
        | Needs at least an email in the filter |

Scenario: Getting single diver with email as required field
    Filtering requirements are met.
    Given Query operation GetDiverByEmailWithRequirement
    When Executed
    Then Should have errors
    And Errors are:
        | Message                               |
        | Needs at least an email in the filter |


Scenario: Getting  divers withou bio
    Filtering on null values
    Given Query operation GetDiverByWithoutBio
    When Executed
    Then No errors
    Then Data contains diver all
        | name | email           | id |
        | Eli  | eli@divers.down | 2  |

Scenario: Getting  divers without birthdate
    Filtering on null values
    Given Query operation GetDiverByWithoutBirthDate
    When Executed
    Then No errors
    Then Data contains diver all
        | name  | id |
        | Harry | 3  |

Scenario: Getting  diver with birthdate 1970
    Filtering on null values
    Given Query operation GetDiverByWithBirthDate
    When Executed
    Then No errors
    Then Data contains diver all
        | name | id |
        | Eli  | 2  |


Scenario: Getting  divers without birthdate and bio
    Filtering on null values
    Given Query operation GetDiverByWithoutBioAndBirtyDate
    When Executed
    Then No errors
    Then Data contains diver all
        | name  | id |
        | Harry | 3  |


Scenario: Getting Divers at location with subfields
    Filtering based on resolve logic,
    Given the following dives for diver 1
        | Location | On         | Start | End   | AverageDepth |
        | Twiske   | 01-01-2023 | 10:00 | 12:00 | 8.5          |
    Given the following dives for diver 2
        | Location  | On         | Start | End   | AverageDepth |
        | Vinkeveen | 01-01-2023 | 10:00 | 12:00 | 8.5          |
    Given Query operation GetDiversWithDivesAt
    Given Variables:
        """
        location: Twiske
        """
    When Executed
    Then No errors
    And Data is not empty
    And Data contains divers exclude dives
        | name |
        | John |

Scenario: Getting Divers thru dataloader at location with subfields
    Filtering based on resolve logic,
    Given the following dives for diver 1
        | Location | On         | Start | End   | AverageDepth |
        | Twiske   | 01-01-2023 | 10:00 | 12:00 | 8.5          |
    Given the following dives for diver 2
        | Location  | On         | Start | End   | AverageDepth |
        | Vinkeveen | 01-01-2023 | 10:00 | 12:00 | 8.5          |
    Given Query operation GetDiversWithDivesDataLoaderAt
    Given Variables:
        """
        location: Twiske
        """
    When Executed
    Then No errors
    And Data is not empty
    And Data contains divers exclude divesWithDataLoader
        | name |
        | John |

Scenario: Getting Divers at location without subfields
    Filtering based on resolve logic, filterin on fields without requesting them
    will not use the filter.
    Given the following dives for diver 1
        | Location | On         | Start | End   | AverageDepth |
        | Twiske   | 01-01-2023 | 10:00 | 12:00 | 8.5          |
    Given the following dives for diver 2
        | Location  | On         | Start | End   | AverageDepth |
        | Vinkeveen | 01-01-2023 | 10:00 | 12:00 | 8.5          |
    Given Query operation GetDiversWithDivesAtNoSubFields
    Given Variables:
        """
        location: Twiske
        """
    When Executed
    Then No errors
    And Data is not empty
    And Data contains divers exclude dives
        | name  |
        | John  |
        | Eli   |
        | Harry |


Scenario: Getting Divers with buddie excluding divers without buddies
    Given the following buddies for diver 1
        | Diver |
        | 2     |
    Given Query operation GetOnlyDiverWithBuddies
    When Executed
    Then No errors
    And Data is not empty
    And Data contains divers exclude buddies
        | name |
        | John |

Scenario: Getting Divers with buddie including divers without buddies
    Given the following buddies for diver 1
        | Diver |
        | 2     |
    Given Query operation GetAllDiversWithOrWithoutBuddies
    When Executed
    Then No errors
    And Data is not empty
    And Data contains divers exclude buddies
        | name  |
        | John  |
        | Eli   |
        | Harry |
