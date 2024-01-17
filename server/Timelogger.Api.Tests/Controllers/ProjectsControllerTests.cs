using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Timelogger.Api.Controllers;
using Timelogger.Api.Tests.Helper;
using Timelogger.Commands;
using Timelogger.Dto;
using Timelogger.Queries;
using Xunit;

namespace Timelogger.Api.Tests.Controllers
{
    public class ProjectsControllerTests
    {
        [Theory]
        [AutoNSubstituteData]
        public async Task Get_ShouldReturnListOfProjects(
            [Frozen] IMediator mediator,
            [NoAutoProperties] ProjectsController sut,
            List<ProjectDto> expectedProjects)
        {
            // Arrange
            mediator.Send(Arg.Any<GetProjectsQuery>())
                .Returns(expectedProjects);

            // Act
            var result = await sut.Get();

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>();
            okResult.Subject.Value.Should().BeEquivalentTo(expectedProjects);
        }

        [Theory]
        [AutoNSubstituteData]
        public async Task AddProject_ShouldReturnOkResult_WhenProjectIsAdded([Frozen] IMediator mediator,
            [NoAutoProperties] ProjectsController sut, ProjectDto projectDto)
        {
            // Arrange
            mediator.Send(Arg.Any<ProjectCommand>())
                .Returns(1);

            // Act
            var result = await sut.AddProject(projectDto);

            // Assert
            var apiResponse = (result.Result as OkObjectResult)?.Value.Should().BeOfType<ApiResponse>();
            apiResponse.Subject.Message.Should().Be("Project added successfully.");

            await mediator.Received(1).Send(Arg.Is<ProjectCommand>(cmd =>
                cmd.Project.Id == projectDto.Id &&
                cmd.Project.Name == projectDto.Name &&
                cmd.Project.Deadline == projectDto.Deadline &&
                cmd.Project.Status == projectDto.Status));
        }
    }
}