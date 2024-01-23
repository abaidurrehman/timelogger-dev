using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Timelogger.Api.Controllers;
using Timelogger.Dto;
using Xunit;

namespace Timelogger.Api.integration.Tests
{
    public class TimeRegistrationControllerIntegrationTests : IntegrationTestBase
    {
        [Fact]
        public async Task GetTimeRegistrations_ShouldReturnListOfTimeRegistrations()
        {
            // Arrange
            await AddSampleTimeRegistrationEntries();

            // Act
            var response = await Client.GetAsync("/api/timeregistration/GetTimesForProject/1");
            response.EnsureSuccessStatusCode();

            // Assert
            var timeRegistrationsJson = await response.Content.ReadAsStringAsync();
            var timeRegistrations = JsonConvert.DeserializeObject<List<TimeRegistrationDto>>(timeRegistrationsJson);
            Assert.NotEmpty(timeRegistrations);
        }

        [Fact]
        public async Task AddTimeRegistration_ShouldReturnOkResponse()
        {
            // Arrange
            var newTimeRegistration = new TimeRegistrationDto
            {
                ProjectId = 1,
                FreelancerId = 1,
                TaskDescription = "New Task",
                StartTime = DateTime.Now.AddHours(-2),
                EndTime = DateTime.Now.AddHours(-1)
            };

            // Act
            var jsonContent = new StringContent(JsonConvert.SerializeObject(newTimeRegistration), Encoding.UTF8,
                "application/json");
            var response = await Client.PostAsync("/api/timeregistration", jsonContent);
            response.EnsureSuccessStatusCode();

            // Assert
            var apiResponseJson = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(apiResponseJson);
            Assert.Equal("Time registration added successfully.", apiResponse.Message);
        }

        [Fact]
        public async Task AddTimeRegistration_ShouldIncreaseTimeRegistrationsCount()
        {
            // Arrange
            await AddSampleTimeRegistrationEntries();

            var initialTimeRegistrationsResponse = await Client.GetAsync("/api/timeregistration/GetTimesForProject/1");
            initialTimeRegistrationsResponse.EnsureSuccessStatusCode();
            var initialTimeRegistrationsJson = await initialTimeRegistrationsResponse.Content.ReadAsStringAsync();
            var initialTimeRegistrations =
                JsonConvert.DeserializeObject<List<TimeRegistrationDto>>(initialTimeRegistrationsJson);
            var initialTimeRegistrationsCount = initialTimeRegistrations.Count;

            var newTimeRegistration = new TimeRegistrationDto
            {
                ProjectId = 1,
                FreelancerId = 1,
                TaskDescription = "New Task",
                StartTime = DateTime.Now.AddHours(-4),
                EndTime = DateTime.Now.AddHours(-3)
            };

            // Act
            var jsonContent = new StringContent(JsonConvert.SerializeObject(newTimeRegistration), Encoding.UTF8,
                "application/json");
            await Client.PostAsync("/api/timeregistration", jsonContent);

            // Assert
            var finalTimeRegistrationsResponse = await Client.GetAsync("/api/timeregistration/GetTimesForProject/1");
            finalTimeRegistrationsResponse.EnsureSuccessStatusCode();
            var finalTimeRegistrationsJson = await finalTimeRegistrationsResponse.Content.ReadAsStringAsync();
            var finalTimeRegistrations =
                JsonConvert.DeserializeObject<List<TimeRegistrationDto>>(finalTimeRegistrationsJson);
            var finalTimeRegistrationsCount = finalTimeRegistrations.Count;

            Assert.Equal(initialTimeRegistrationsCount + 1, finalTimeRegistrationsCount);
        }

        private async Task AddSampleTimeRegistrationEntries()
        {
            var sampleTimeRegistrations = new List<TimeRegistrationDto>
            {
                new TimeRegistrationDto
                {
                    ProjectId = 1,
                    FreelancerId = 1,
                    TaskDescription = "Sample Task 1",
                    StartTime = DateTime.Now.AddHours(-8),
                    EndTime = DateTime.Now.AddHours(-7)
                },
                new TimeRegistrationDto
                {
                    ProjectId = 1,
                    FreelancerId = 1,
                    TaskDescription = "Sample Task 2",
                    StartTime = DateTime.Now.AddHours(-10),
                    EndTime = DateTime.Now.AddHours(-9)
                }
                // Add more entries as needed
            };

            foreach (var timeRegistration in sampleTimeRegistrations)
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(timeRegistration), Encoding.UTF8,
                    "application/json");
                var apiResponseJson = await Client.PostAsync("/api/timeregistration", jsonContent);
            }
        }
    }
}