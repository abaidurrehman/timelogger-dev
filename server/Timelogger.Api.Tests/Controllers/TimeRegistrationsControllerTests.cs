using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Timelogger.Api.Controllers;
using Timelogger.Api.Tests.Helper;
using Timelogger.Commands;
using Timelogger.Dto;
using Timelogger.Queries;
using Xunit;

namespace Timelogger.Api.Tests.Controllers
{
    public class TimeRegistrationControllerTests
    {
        [Theory]
        [AutoNSubstituteData]
        public async Task AddTimeRegistration_ShouldReturnOkResult_WhenSuccessful(
            [Frozen] IMediator mediator,
            [NoAutoProperties] TimeRegistrationController sut,
            TimeRegistrationDto timeRegistrationDto)
        {
            // Arrange
            mediator.Send(Arg.Any<AddTimeRegistrationCommand>())
                .Returns(CommandResult.Success("Time registration added successfully."));

            // Act
            var result = await sut.AddTimeRegistration(timeRegistrationDto);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>();
            okResult.Subject.Value.Should().BeOfType<ApiResponse>();
            okResult.Subject.Value.As<ApiResponse>().Message.Should().Be("Time registration added successfully.");
        }

        [Theory]
        [AutoNSubstituteData]
        public async Task AddTimeRegistration_ShouldReturnBadRequestResult_WhenValidationFails(
            [Frozen] IMediator mediator,
            [NoAutoProperties] TimeRegistrationController sut,
            TimeRegistrationDto timeRegistrationDto)
        {
            // Arrange
            var validationFailure = "End time should be greater than start time.";
            mediator.Send(Arg.Any<AddTimeRegistrationCommand>())
                .Returns(CommandResult.Fail(validationFailure));

            // Act
            var result = await sut.AddTimeRegistration(timeRegistrationDto);

            // Assert
            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult.Subject.Value.Should().BeOfType<ApiResponse>();
            badRequestResult.Subject.Value.As<ApiResponse>().Errors.Should().Contain(validationFailure);
        }

        [Theory]
        [AutoNSubstituteData]
        public async Task AddTimeRegistration_ShouldReturnInternalServerError_WhenExceptionOccurs(
            [Frozen] IMediator mediator,
            [NoAutoProperties] TimeRegistrationController sut,
            TimeRegistrationDto timeRegistrationDto)
        {
            // Arrange
            mediator.Send(Arg.Any<AddTimeRegistrationCommand>())
                .Throws(new Exception("exception"));

            // Act
            var result = await sut.AddTimeRegistration(timeRegistrationDto);

            // Assert
            var internalServerErrorResult = result.Result.Should().BeOfType<ObjectResult>();
            internalServerErrorResult.Subject.StatusCode.Should().Be(500);
            internalServerErrorResult.Subject.Value.Should().BeOfType<ApiResponse>();
            internalServerErrorResult.Subject.Value.As<ApiResponse>().Message.Should()
                .Be("An error occurred while processing the request.");
        }

        [Theory]
        [AutoNSubstituteData]
        public async Task GetTimesForProject_ShouldReturnOkResult_WhenSuccessful(
            [Frozen] IMediator mediator,
            [NoAutoProperties] TimeRegistrationController sut,
            int projectId,
            List<TimeRegistrationDto> timeRegistrations)
        {
            // Arrange
            mediator.Send(Arg.Any<GetTimeRegistrationQueryQuery>())
                .Returns(timeRegistrations);

            // Act
            var result = await sut.GetTimesForProject(projectId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okObjectResult = result.Result.As<OkObjectResult>();
            okObjectResult.Value.Should().BeEquivalentTo(timeRegistrations);
        }

        [Theory]
        [AutoNSubstituteData]
        public async Task GetTimesForProject_ShouldReturnInternalServerError_WhenExceptionOccurs(
            [Frozen] IMediator mediator,
            [NoAutoProperties] TimeRegistrationController sut,
            int projectId)
        {
            // Arrange
            mediator.Send(Arg.Any<GetTimeRegistrationQueryQuery>())
                .Throws(new Exception("exception during processing"));

            // Act
            var result = await sut.GetTimesForProject(projectId);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var internalServerErrorResult = result.Result.As<ObjectResult>();
            internalServerErrorResult.StatusCode.Should().Be(500);
            internalServerErrorResult.Value.As<ApiResponse>().Message.Should()
                .Be("An error occurred while processing the request.");
        }
    }
}