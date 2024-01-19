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
    public class TimeRegistrationRepositoryTests
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
        public async Task GetTimeRegistrationsForProjectAsync_ReturnsTimeRegistrations(
            List<TimeRegistration> timeRegistrations, int projectId)
        {
            // Arrange
            timeRegistrations.ForEach(tr => tr.ProjectId = projectId);
            using (var dbContextInMemory = CreateInMemoryDbContext())
            {
                dbContextInMemory.TimeRegistrations.AddRange(timeRegistrations);
                await dbContextInMemory.SaveChangesAsync();

                var timeRegistrationRepository = new TimeRegistrationRepository(dbContextInMemory);

                // Act
                var result =
                    await timeRegistrationRepository.GetTimeRegistrationsForProjectAsync(projectId,
                        CancellationToken.None);

                // Assert
                result.Should().NotBeNull();
                result.Should().BeEquivalentTo(timeRegistrations);
            }
        }

        [Theory]
        [AutoNSubstituteData]
        public async Task AddTimeRegistrationAsync_AddsTimeRegistration(TimeRegistration timeRegistration)
        {
            // Arrange
            using (var dbContextInMemory = CreateInMemoryDbContext())
            {
                var timeRegistrationRepository = new TimeRegistrationRepository(dbContextInMemory);

                // Act
                var result =
                    await timeRegistrationRepository.AddTimeRegistrationAsync(timeRegistration, CancellationToken.None);

                // Assert
                result.Should().BeGreaterThan(0);
                dbContextInMemory.TimeRegistrations.Should().ContainEquivalentOf(timeRegistration);
            }
        }


        [Theory]
        [AutoNSubstituteData]
        public async Task IsDuplicateTimeRegistrationAsync_ReturnsTrue_WhenOverlapCompletely(
            List<TimeRegistration> existingTimeRegistrations, int projectId)
        {
            // Arrange
            var startTime = DateTime.UtcNow;
            var endTime = startTime.AddHours(2);

            var completelyOverlappingRegistration = new TimeRegistration
            {
                ProjectId = projectId,
                StartTime = startTime,
                EndTime = endTime
            };
            existingTimeRegistrations.Add(completelyOverlappingRegistration);

            using (var dbContextInMemory = CreateInMemoryDbContext())
            {
                dbContextInMemory.TimeRegistrations.AddRange(existingTimeRegistrations);
                await dbContextInMemory.SaveChangesAsync();

                var timeRegistrationRepository = new TimeRegistrationRepository(dbContextInMemory);

                // Act
                var result =
                    await timeRegistrationRepository.IsDuplicateTimeRegistrationAsync(projectId, startTime, endTime,
                        CancellationToken.None);

                // Assert
                result.Should().BeTrue();
            }
        }

        [Theory]
        [AutoNSubstituteData]
        public async Task IsDuplicateTimeRegistrationAsync_ReturnsTrue_WhenOverlapPartially(
            List<TimeRegistration> existingTimeRegistrations, int projectId)
        {
            // Arrange
            var startTime = DateTime.UtcNow;
            var endTime = startTime.AddHours(2);

            var partiallyOverlappingRegistration = new TimeRegistration
            {
                ProjectId = projectId,
                StartTime = startTime.AddMinutes(1),
                EndTime = endTime.AddMinutes(-1)
            };
            existingTimeRegistrations.Add(partiallyOverlappingRegistration);

            using (var dbContextInMemory = CreateInMemoryDbContext())
            {
                dbContextInMemory.TimeRegistrations.AddRange(existingTimeRegistrations);
                await dbContextInMemory.SaveChangesAsync();

                var timeRegistrationRepository = new TimeRegistrationRepository(dbContextInMemory);

                // Act
                var result =
                    await timeRegistrationRepository.IsDuplicateTimeRegistrationAsync(projectId, startTime, endTime,
                        CancellationToken.None);

                // Assert
                result.Should().BeTrue();
            }
        }

        [Theory]
        [AutoNSubstituteData]
        public async Task IsDuplicateTimeRegistrationAsync_ReturnsTrue_WhenWithinExisting(
            List<TimeRegistration> existingTimeRegistrations, int projectId)
        {
            // Arrange
            var startTime = DateTime.UtcNow;
            var endTime = startTime.AddHours(2);

            var withinExistingRegistration = new TimeRegistration
            {
                ProjectId = projectId,
                StartTime = startTime.AddMinutes(2),
                EndTime = endTime.AddMinutes(-2)
            };

            existingTimeRegistrations.Add(withinExistingRegistration);

            using (var dbContextInMemory = CreateInMemoryDbContext())
            {
                dbContextInMemory.TimeRegistrations.AddRange(existingTimeRegistrations);
                await dbContextInMemory.SaveChangesAsync();

                var timeRegistrationRepository = new TimeRegistrationRepository(dbContextInMemory);

                // Act
                var result =
                    await timeRegistrationRepository.IsDuplicateTimeRegistrationAsync(projectId, startTime, endTime,
                        CancellationToken.None);

                // Assert
                result.Should().BeTrue();
            }
        }

        [Theory]
        [AutoNSubstituteData]
        public async Task IsDuplicateTimeRegistrationAsync_ReturnsFalse_WhenNoOverlap(
            List<TimeRegistration> existingTimeRegistrations, int projectId)
        {
            // Arrange
            var startTime = DateTime.UtcNow;
            var endTime = startTime.AddHours(2);

            var noOverlapRegistration = new TimeRegistration
            {
                ProjectId = projectId,
                StartTime = endTime.AddMinutes(1),
                EndTime = endTime.AddMinutes(2)
            };
            existingTimeRegistrations.Add(noOverlapRegistration);

            using (var dbContextInMemory = CreateInMemoryDbContext())
            {
                dbContextInMemory.TimeRegistrations.AddRange(existingTimeRegistrations);
                await dbContextInMemory.SaveChangesAsync();

                var timeRegistrationRepository = new TimeRegistrationRepository(dbContextInMemory);

                // Act
                var result =
                    await timeRegistrationRepository.IsDuplicateTimeRegistrationAsync(projectId, startTime, endTime,
                        CancellationToken.None);

                // Assert
                result.Should().BeFalse();
            }
        }
    }
}