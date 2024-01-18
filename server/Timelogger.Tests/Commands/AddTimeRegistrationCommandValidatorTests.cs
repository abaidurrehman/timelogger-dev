using System;
using AutoFixture.Xunit2;
using FluentValidation.TestHelper;
using Timelogger.Commands;
using Timelogger.Dto;
using Timelogger.Tests.Helper;
using Xunit;

namespace Timelogger.Tests.Commands
{
    public class AddTimeRegistrationCommandValidatorTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void ShouldHaveValidationError_WhenProjectIdIsNotGreaterThanZero(
            [NoAutoProperties] AddTimeRegistrationCommandValidator sut)
        {
            // Arrange
            var command = new AddTimeRegistrationCommand
            {
                TimeRegistration = new TimeRegistrationDto
                {
                    ProjectId = 0, // Setting ProjectId to an invalid value
                    FreelancerId = 1,
                    TaskDescription = "Sample Task"
                }
            };

            // Act
            var result = sut.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(v => v.TimeRegistration.ProjectId)
                .WithErrorMessage("Project is required.");
        }

        [Theory]
        [AutoNSubstituteData]
        public void ShouldHaveValidationError_WhenFreelancerIdIsNotGreaterThanZero(
            [NoAutoProperties] AddTimeRegistrationCommandValidator sut)
        {
            // Arrange
            var command = new AddTimeRegistrationCommand
            {
                TimeRegistration = new TimeRegistrationDto
                {
                    ProjectId = 1,
                    FreelancerId = 0,
                    TaskDescription = "Sample Task"
                }
            };

            // Act
            var result = sut.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(v => v.TimeRegistration.FreelancerId)
                .WithErrorMessage("Freelancer is required.");
        }

        [Theory]
        [AutoNSubstituteData]
        public void ShouldHaveValidationError_WhenTaskDescriptionIsNullOrEmpty(
            [NoAutoProperties] AddTimeRegistrationCommandValidator sut)
        {
            // Arrange
            var command = new AddTimeRegistrationCommand
            {
                TimeRegistration = new TimeRegistrationDto
                {
                    ProjectId = 1,
                    FreelancerId = 1,
                    TaskDescription = string.Empty
                }
            };

            // Act
            var result = sut.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(v => v.TimeRegistration.TaskDescription)
                .WithErrorMessage("Task description is required.");
        }

        [Theory]
        [AutoNSubstituteData]
        public void ShouldHaveValidationError_WhenEndTimeIsBeforeStartTime(
            AddTimeRegistrationCommandValidator sut)
        {
            // Arrange
            var command = new AddTimeRegistrationCommand
            {
                TimeRegistration = new TimeRegistrationDto
                {
                    ProjectId = 1, 
                    FreelancerId = 1,
                    TaskDescription = "Sample Task",
                    StartTime = DateTime.Now.AddHours(2),
                    EndTime = DateTime.Now.AddHours(1)
                }
            };

            // Act
            var result = sut.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(v => v.TimeRegistration.StartTime)
                .WithErrorMessage("End time should be greater than start time.");
        }

        [Theory]
        [AutoNSubstituteData]
        public void ShouldHaveValidationError_WhenStartTimeIsInFuture(
            [NoAutoProperties] AddTimeRegistrationCommandValidator sut)
        {
            // Arrange
            var command = new AddTimeRegistrationCommand
            {
                TimeRegistration = new TimeRegistrationDto
                {
                    ProjectId = 1,
                    FreelancerId = 1,
                    TaskDescription = "Sample Task",
                    StartTime = DateTime.Now.AddHours(1),
                    EndTime = DateTime.Now.AddHours(2)
                }
            };

            // Act
            var result = sut.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(v => v.TimeRegistration.StartTime)
                .WithErrorMessage("Start time cannot be in the future.");
        }

        [Theory]
        [AutoNSubstituteData]
        public void ShouldHaveValidationError_WhenEndTimeIsInFuture(
            [NoAutoProperties] AddTimeRegistrationCommandValidator sut)
        {
            // Arrange
            var command = new AddTimeRegistrationCommand
            {
                TimeRegistration = new TimeRegistrationDto
                {
                    ProjectId = 1,
                    FreelancerId = 1,
                    TaskDescription = "Sample Task",
                    StartTime = DateTime.Now.AddHours(-2),
                    EndTime = DateTime.Now.AddHours(1)
                }
            };

            // Act
            var result = sut.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(v => v.TimeRegistration.EndTime)
                .WithErrorMessage("End time cannot be in the future.");
        }

        [Theory]
        [AutoNSubstituteData]
        public void ShouldHaveValidationErrors_WhenMultipleValidationFailures(
            [NoAutoProperties] AddTimeRegistrationCommandValidator sut)
        {
            // Arrange
            var command = new AddTimeRegistrationCommand
            {
                TimeRegistration = new TimeRegistrationDto
                {
                    StartTime = DateTime.Now.AddHours(1),
                    EndTime = DateTime.Now.AddMinutes(29)
                }
            };

            // Act
            var result = sut.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(v => v.TimeRegistration.ProjectId)
                .WithErrorMessage("Project is required.");

            result.ShouldHaveValidationErrorFor(v => v.TimeRegistration.FreelancerId)
                .WithErrorMessage("Freelancer is required.");

            result.ShouldHaveValidationErrorFor(v => v.TimeRegistration.TaskDescription)
                .WithErrorMessage("Task description is required.");

            result.ShouldHaveValidationErrorFor(v =>
                    (v.TimeRegistration.EndTime - v.TimeRegistration.StartTime).TotalMinutes)
                .WithErrorMessage("Duration between Start Time and End Time should be at least 30 minutes.");

            result.ShouldHaveValidationErrorFor(v => v.TimeRegistration.StartTime)
                .WithErrorMessage("Start time cannot be in the future.");

            result.ShouldHaveValidationErrorFor(v => v.TimeRegistration.EndTime)
                .WithErrorMessage("End time cannot be in the future.");
        }
    }
}