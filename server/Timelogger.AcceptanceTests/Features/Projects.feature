Feature: Time Logger Projects

  As a freelancer
  I want to be able to log my time on projects
  So that I can track my work and generate invoices

  Scenario: View Time Registrations
    Given I am logged in as a freelancer
    And I navigate to the "timelogger" application
    When I view the time registrations for the project "Project A"
    Then I should see all time registrations for that project

  Scenario: Sort Projects by Deadline
    Given I am logged in as a freelancer
    And I navigate to the "timelogger" application
    When I sort projects by their deadline
    Then the projects should be sorted in ascending order of their deadlines

  Scenario: Add New Project
    Given I am logged in as a freelancer
    And I navigate to the "timelogger" application
    When I add a new project with the name "Project B" and a deadline of "2024-03-01"
    Then the project should be added successfully
