//using AutoFixture;
//using FluentAssertions;
//using Microsoft.EntityFrameworkCore;
//using NSubstitute;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using Timelogger.Data.Repositories;
//using Timelogger.Data;
//using Timelogger.Entities;
//using Xunit;

//public class ProjectRepositoryTests
//{
//    [Fact]
//    public async Task GetAllProjectsAsync_ShouldReturnAllProjects()
//    {
//        // Arrange
//        var fixture = new Fixture();
//        var projects = fixture.CreateMany<Project>().ToList();

//        var mockSet = Substitute.For<DbSet<Project>, IQueryable<Project>>();
//        mockSet.Provider.Returns(projects.AsQueryable().Provider);
//        mockSet.Expression.Returns(projects.AsQueryable().Expression);
//        mockSet.ElementType.Returns(projects.AsQueryable().ElementType);
//        mockSet.GetEnumerator().Returns(projects.AsQueryable().GetEnumerator());

//        var mockContext = Substitute.For<TimeloggerDbContext>();
//        mockContext.Projects.Returns(mockSet);

//        var repository = new ProjectRepository(mockContext);

//        // Act
//        var result = await repository.GetAllProjectsAsync(CancellationToken.None);

//        // Assert
//        result.Should().BeEquivalentTo(projects);
//    }

//    [Fact]
//    public async Task GetProjectByIdAsync_ShouldReturnProjectById()
//    {
//        // Arrange
//        var fixture = new Fixture();
//        var projects = fixture.CreateMany<Project>().ToList();

//        var mockSet = Substitute.For<DbSet<Project>>();
//        mockSet.FindAsync(Arg.Any<object[]>()).Returns((object[] ids) =>
//        {
//            var projectId = (int)ids[0];
//            return Task.FromResult(projects.FirstOrDefault(p => p.Id == projectId));
//        });

//        var mockContext = Substitute.For<TimeloggerDbContext>();
//        mockContext.Projects.Returns(mockSet);

//        var repository = new ProjectRepository(mockContext);

//        // Act
//        var result = await repository.GetProjectByIdAsync(2, CancellationToken.None);

//        // Assert
//        result.Should().NotBeNull();
//        result.Id.Should().Be(2);
//    }

//    [Fact]
//    public async Task AddProjectAsync_ShouldAddProject()
//    {
//        // Arrange
//        var fixture = new Fixture();
//        var project = fixture.Create<Project>();

//        var mockSet = Substitute.For<DbSet<Project>>();
//        var mockContext = Substitute.For<TimeloggerDbContext>();
//        mockContext.Projects.Returns(mockSet);

//        var repository = new ProjectRepository(mockContext);

//        // Act
//        var result = await repository.AddProjectAsync(project, CancellationToken.None);

//        // Assert
//        mockSet.Received(1).Add(project);
//        await mockContext.Received(1).SaveChangesAsync(CancellationToken.None);
//        result.Should().Be(project.Id);
//    }
//}
