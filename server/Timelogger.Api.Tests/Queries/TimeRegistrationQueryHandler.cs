using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Timelogger.Api.Tests.Helper;
using Timelogger.Data.Repositories;
using Timelogger.Dto;
using Timelogger.Entities;
using Timelogger.Queries;
using Xunit;

namespace Timelogger.Api.Tests.Queries
{
    public class TimeRegistrationQueryHandlerTests
    {
        [Theory]
        [AutoNSubstituteData]
        public async Task Handle_ShouldReturnMappedTimeRegistrations(
            [Frozen] ITimeRegistrationRepository timeRegistrationRepository,
            [Frozen] IMapper mapper,
            TimeRegistrationQueryHandler sut,
            GetTimeRegistrationQueryQuery query,
            List<TimeRegistration> timeRegistrationsFromRepository,
            List<TimeRegistrationDto> mappedTimeRegistrations)
        {
            // Arrange
            timeRegistrationRepository
                .GetTimeRegistrationsForProjectAsync(query.ProjectId, Arg.Any<CancellationToken>())
                .Returns(timeRegistrationsFromRepository);

            mapper.Map<IEnumerable<TimeRegistrationDto>>(timeRegistrationsFromRepository)
                .Returns(mappedTimeRegistrations);

            // Act
            var result = await sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(mappedTimeRegistrations);
        }

        [Theory]
        [AutoNSubstituteData]
        public async Task Handle_ShouldReturnEmptyList_WhenRepositoryReturnsEmptyList(
            [Frozen] ITimeRegistrationRepository timeRegistrationRepository,
            TimeRegistrationQueryHandler sut,
            GetTimeRegistrationQueryQuery query)
        {
            // Arrange
            timeRegistrationRepository
                .GetTimeRegistrationsForProjectAsync(query.ProjectId, Arg.Any<CancellationToken>())
                .Returns(new List<TimeRegistration>());


            // Act
            var result = await sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }
    }
}