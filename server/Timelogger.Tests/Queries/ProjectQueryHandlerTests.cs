using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Timelogger.Data.Repositories;
using Timelogger.Dto;
using Timelogger.Entities;
using Timelogger.Queries;
using Timelogger.Tests.Helper;
using Xunit;

namespace Timelogger.Tests.Queries
{
    public class ProjectQueryHandlerTests
    {
        [Theory]
        [AutoNSubstituteData]
        public async Task Handle_ShouldReturnMappedProjects(
            [Frozen] IProjectRepository projectRepository,
            [Frozen] IMapper mapper,
            ProjectQueryHandler sut,
            GetProjectsQuery query,
            List<Project> projectsFromRepository,
            List<ProjectDto> mappedProjects)
        {
            // Arrange
            projectRepository.GetAllProjectsAsync(Arg.Any<CancellationToken>())
                .Returns(projectsFromRepository);
            mapper.Map<IEnumerable<ProjectDto>>(projectsFromRepository)
                .Returns(mappedProjects);

            // Act
            var result = await sut.Handle(query, CancellationToken.None);

            // Assert
            await projectRepository.Received(1).GetAllProjectsAsync(Arg.Any<CancellationToken>());
            result.Should().BeEquivalentTo(mappedProjects);
        }


        [Theory]
        [AutoNSubstituteData]
        public async Task Handle_ShouldMapProjectsUsingMapper(
            [Frozen] IProjectRepository projectRepository,
            [Frozen] IMapper mapper,
            ProjectQueryHandler sut,
            GetProjectsQuery query,
            IEnumerable<Project> projectsFromRepository)
        {
            // Arrange
            projectRepository.GetAllProjectsAsync(Arg.Any<CancellationToken>())
                .Returns(projectsFromRepository);

            // Act
            await sut.Handle(query, CancellationToken.None);

            // Assert
            mapper.Received(1).Map<IEnumerable<ProjectDto>>(projectsFromRepository);
        }

        [Theory]
        [AutoNSubstituteData]
        public async Task Handle_ShouldReturnEmptyList_WhenRepositoryReturnsNull(
            [Frozen] IProjectRepository projectRepository,
            [Frozen] IMapper mapper,
            ProjectQueryHandler sut,
            GetProjectsQuery query)
        {
            // Arrange
            projectRepository.GetAllProjectsAsync(Arg.Any<CancellationToken>())
                .Returns((IEnumerable<Project>)null);
            mapper.Map<IEnumerable<ProjectDto>>(null)
                .Returns(Enumerable.Empty<ProjectDto>());

            // Act
            var result = await sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }
    }
}