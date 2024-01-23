using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Timelogger.Api.Controllers;
using Timelogger.Dto;
using Timelogger.Entities;
using Xunit;

namespace Timelogger.Api.integration.Tests
{
    public class ProjectsControllerIntegrationTests : IntegrationTestBase
    {
        [Fact]
        public async Task GetProjects_ShouldReturnListOfProjects()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync("/api/projects");
            response.EnsureSuccessStatusCode();

            // Assert
            var projectsJson = await response.Content.ReadAsStringAsync();
            var projects = JsonConvert.DeserializeObject<List<ProjectDto>>(projectsJson);
            Assert.NotEmpty(projects);
        }

        [Fact]
        public async Task AddProject_ShouldReturnOkResponse()
        {
            // Arrange
            var newProject = new ProjectDto
            {
                Name = "New Project",
                Deadline = DateTime.Now.AddDays(30),
                Status = ProjectStatus.New
            };

            // Act
            var jsonContent = new StringContent(JsonConvert.SerializeObject(newProject), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync("/api/projects", jsonContent);
            response.EnsureSuccessStatusCode();

            // Assert
            var apiResponseJson = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(apiResponseJson);
            Assert.Equal("Project added successfully.", apiResponse.Message);
        }

        [Fact]
        public async Task AddProject_ShouldIncreaseProjectCount()
        {
            // Arrange
            var initialProjectsResponse = await Client.GetAsync("/api/projects");
            initialProjectsResponse.EnsureSuccessStatusCode();
            var initialProjectsJson = await initialProjectsResponse.Content.ReadAsStringAsync();
            var initialProjects = JsonConvert.DeserializeObject<List<ProjectDto>>(initialProjectsJson);
            var initialProjectCount = initialProjects.Count;

            var newProject = new ProjectDto
            {
                Name = "New Project",
                Deadline = DateTime.Now.AddDays(30),
                Status = ProjectStatus.New
            };

            // Act
            var jsonContent = new StringContent(JsonConvert.SerializeObject(newProject), Encoding.UTF8, "application/json");
            await Client.PostAsync("/api/projects", jsonContent);

            // Assert
            var finalProjectsResponse = await Client.GetAsync("/api/projects");
            finalProjectsResponse.EnsureSuccessStatusCode();
            var finalProjectsJson = await finalProjectsResponse.Content.ReadAsStringAsync();
            var finalProjects = JsonConvert.DeserializeObject<List<ProjectDto>>(finalProjectsJson);
            var finalProjectCount = finalProjects.Count;

            Assert.Equal(initialProjectCount + 1, finalProjectCount);
        }
    }
}
