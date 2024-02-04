Feature: Time Registration Form
    As a freelancer
    I want to be able to fill out and submit the time registration form
    So that my time can be logged accurately

Scenario: Fill out and submit the time registration form with valid data
    Given I am on the time registration form page
    When I fill out the form with valid data
    And I submit the form
    Then the form should be submitted successfully

Scenario: Cancel the time registration form
    Given I am on the time registration form page
    When I click the cancel button
    Then the form should be closed

Scenario: Fill out the form with invalid data
    Given I am on the time registration form page
    When I fill out the form with invalid data
    And I submit the form
    Then I should see error messages displayed
    And the form should not be submitted

Scenario: View projects in the dropdown list
    Given I am on the time registration form page
    Then I should see a dropdown list of available projects

Scenario: Fill out the form with missing project
    Given I am on the time registration form page
    When I fill out the form without selecting a project
    And I submit the form
    Then I should see an error message indicating that the project is required

Scenario: Fill out the form with missing task description
    Given I am on the time registration form page
    When I fill out the form without providing a task description
    And I submit the form
    Then I should see an error message indicating that the task description is required

Scenario: Fill out the form with invalid start time
    Given I am on the time registration form page
    When I fill out the form with an invalid start time
    And I submit the form
    Then I should see an error message indicating that the start time is invalid

Scenario: Fill out the form with invalid end time
    Given I am on the time registration form page
    When I fill out the form with an invalid end time
    And I submit the form
    Then I should see an error message indicating that the end time is invalid

Scenario: Fill out the form with end time before start time
    Given I am on the time registration form page
    When I fill out the form with an end time before the start time
    And I submit the form
    Then I should see an error message indicating that the end time must be after the start time

Scenario: Fill out the form with end time less than 30 minutes after start time
    Given I am on the time registration form page
    When I fill out the form with an end time less than 30 minutes after the start time
    And I submit the form
    Then I should see an error message indicating that the duration must be at least 30 minutes

Scenario: Fill out the form with past start time
    Given I am on the time registration form page
    When I fill out the form with a start time in the past
    And I submit the form
    Then I should see an error message indicating that the start time cannot be in the past

Scenario: Fill out the form with past end time
    Given I am on the time registration form page
    When I fill out the form with an end time in the past
    And I submit the form
    Then I should see an error message indicating that the end time cannot be in the past

Scenario: Fill out the form with future start time
    Given I am on the time registration form page
    When I fill out the form with a start time in the future
    And I submit the form
    Then I should see an error message indicating that the start time cannot be in the future

Scenario: Fill out the form with future end time
    Given I am on the time registration form page
    When I fill out the form with an end time in the future
    And I submit the form
    Then I should see an error message indicating that the end time cannot be in the future
