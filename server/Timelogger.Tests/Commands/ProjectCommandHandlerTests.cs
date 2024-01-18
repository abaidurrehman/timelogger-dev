using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Timelogger.Commands;
using Timelogger.Data.Repositories;
using Timelogger.Entities;
using Timelogger.Tests.Helper;
using Xunit;

namespace Timelogger.Tests.Commands
{
    public class ProjectCommandHandlerTests
    {
        [Theory]
        [AutoNSubstituteData]
        public async Task Handle_ShouldReturnAddedProjectId(
            [Frozen] IProjectRepository projectRepository,
            [Frozen] IMapper mapper,
            ProjectCommandHandler handler,
            ProjectCommand command,
            Project projectEntity,
            int projectId)
        {
            // Arrange
            mapper.Map<Project>(command.Project)
                .Returns(projectEntity);

            projectRepository.AddProjectAsync(projectEntity, Arg.Any<CancellationToken>())
                .Returns(projectId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(projectId);
        }

        [Theory]
        [AutoNSubstituteData]
        public async Task Handle_ShouldMapProjectDtoToProjectEntity(
            [Frozen] IProjectRepository projectRepository,
            [Frozen] IMapper mapper,
            ProjectCommandHandler handler,
            ProjectCommand command,
            Project projectEntity,
            int projectId)
        {
            // Arrange
            mapper.Map<Project>(command.Project)
                .Returns(projectEntity);

            projectRepository.AddProjectAsync(Arg.Any<Project>(), Arg.Any<CancellationToken>())
                .Returns(projectId);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            mapper.Received(1).Map<Project>(command.Project);
        }

        [Theory]
        [AutoNSubstituteData]
        public async Task Handle_ShouldCallAddProjectAsyncOnRepository(
            [Frozen] IProjectRepository projectRepository,
            [Frozen] IMapper mapper,
            ProjectCommandHandler handler,
            ProjectCommand command,
            Project projectEntity)
        {
            // Arrange
            mapper.Map<Project>(command.Project)
                .Returns(projectEntity);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            await projectRepository.Received(1).AddProjectAsync(projectEntity, Arg.Any<CancellationToken>());
        }
    }
}