using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Timelogger.Data;
using Timelogger.Data.Repositories;
using Timelogger.Entities;
using Timelogger.Tests.Helper;
using Xunit;

namespace Timelogger.Tests.Data.Repositories
{
    public class ProjectRepositoryTests
    {
        private TimeloggerDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<TimeloggerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TimeloggerDbContext(options);
        }

        [Theory]
        [AutoNSubstituteData]
        public async Task GetAllProjectsAsync_ReturnsProjects(List<Project> projects)
        {
            // Arrange
            using (var dbContextInMemory = CreateInMemoryDbContext())
            {
                dbContextInMemory.Projects.AddRange(projects);
                await dbContextInMemory.SaveChangesAsync();

                var projectRepository = new ProjectRepository(dbContextInMemory);

                // Act
                var result = await projectRepository.GetAllProjectsAsync(CancellationToken.None);

                // Assert
                result.Should().NotBeNull();
                result.Should().HaveCount(projects.Count);
                result.Should().BeEquivalentTo(projects);
            }
        }

        [Theory]
        [AutoNSubstituteData]
        public async Task GetProjectByIdAsync_ReturnsProject(Project project)
        {
            // Arrange
            await using (var dbContextInMemory = CreateInMemoryDbContext())
            {
                dbContextInMemory.Projects.Add(project);
                await dbContextInMemory.SaveChangesAsync();

                var projectRepository = new ProjectRepository(dbContextInMemory);

                // Act
                var result = await projectRepository.GetProjectByIdAsync(project.Id, CancellationToken.None);

                // Assert
                result.Should().NotBeNull();
                result.Should().BeEquivalentTo(project);
            }
        }

        [Theory]
        [AutoNSubstituteData]
        public async Task AddProjectAsync_AddsProject(Project project)
        {
            // Arrange
            using (var dbContextInMemory = CreateInMemoryDbContext())
            {
                var projectRepository = new ProjectRepository(dbContextInMemory);

                // Act
                var result = await projectRepository.AddProjectAsync(project, CancellationToken.None);

                // Assert
                result.Should().BeGreaterThan(0);
                dbContextInMemory.Projects.Should().ContainEquivalentOf(project);
            }
        }
    }
}