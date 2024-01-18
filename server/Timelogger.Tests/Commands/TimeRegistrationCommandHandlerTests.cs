using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Timelogger.Commands;
using Timelogger.Data.Repositories;
using Timelogger.Dto;
using Timelogger.Entities;
using Timelogger.Tests.Helper;
using Xunit;

namespace Timelogger.Tests.Commands
{
    public class TimeRegistrationCommandHandlerTests
    {
        [Theory]
        [AutoNSubstituteData]
        public async Task Handle_ShouldAddTimeRegistration_WhenNotDuplicateAndProjectNotComplete(
            [Frozen] ITimeRegistrationRepository timeRegistrationRepository,
            [Frozen] IProjectRepository projectRepository,
            [Frozen] IMapper mapper,
            TimeRegistrationCommandHandler handler,
            AddTimeRegistrationCommand command,
            TimeRegistration timeRegistrationEntity,
            Project project)
        {
            // Arrange
            timeRegistrationRepository
                .IsDuplicateTimeRegistrationAsync(Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(),
                    Arg.Any<CancellationToken>())
                .Returns(false);

            projectRepository.GetProjectByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(project);

            mapper.Map<TimeRegistration>(Arg.Any<TimeRegistrationDto>()).Returns(timeRegistrationEntity);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(CommandResult.Success("Time registration added successfully."));

            await timeRegistrationRepository.Received(1)
                .AddTimeRegistrationAsync(Arg.Is<TimeRegistration>(tr =>
                        tr.ProjectId == timeRegistrationEntity.ProjectId &&
                        tr.StartTime == timeRegistrationEntity.StartTime &&
                        tr.EndTime == timeRegistrationEntity.EndTime),
                    Arg.Any<CancellationToken>());
        }

        [Theory]
        [AutoNSubstituteData]
        public async Task Handle_ShouldFail_WhenDuplicateTimeRegistration(
            [Frozen] ITimeRegistrationRepository timeRegistrationRepository,
            [Frozen] IMapper mapper,
            TimeRegistrationCommandHandler handler,
            TimeRegistration timeRegistrationEntity,
            AddTimeRegistrationCommand command)
        {
            // Arrange
            timeRegistrationRepository
                .IsDuplicateTimeRegistrationAsync(Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(),
                    Arg.Any<CancellationToken>())
                .Returns(true);

            mapper.Map<TimeRegistration>(Arg.Any<TimeRegistrationDto>()).Returns(timeRegistrationEntity);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(CommandResult.Fail("Duplicate time registration for the same project."));

            await timeRegistrationRepository.DidNotReceiveWithAnyArgs()
                .AddTimeRegistrationAsync(Arg.Any<TimeRegistration>(), Arg.Any<CancellationToken>());
        }

        [Theory]
        [AutoNSubstituteData]
        public async Task Handle_ShouldFail_WhenProjectComplete(
            [Frozen] ITimeRegistrationRepository timeRegistrationRepository,
            [Frozen] IProjectRepository projectRepository,
            [Frozen] IMapper mapper,
            TimeRegistrationCommandHandler handler,
            AddTimeRegistrationCommand command,
            TimeRegistration timeRegistrationEntity,
            Project project)
        {
            // Arrange
            timeRegistrationRepository
                .IsDuplicateTimeRegistrationAsync(Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(),
                    Arg.Any<CancellationToken>())
                .Returns(false);

            projectRepository.GetProjectByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(project);

            mapper.Map<TimeRegistration>(Arg.Any<TimeRegistrationDto>()).Returns(timeRegistrationEntity);

            project.Status = ProjectStatus.Complete;


            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(CommandResult.Fail("Cannot add time registration to a completed project."));

            await timeRegistrationRepository.DidNotReceiveWithAnyArgs()
                .AddTimeRegistrationAsync(Arg.Any<TimeRegistration>(), Arg.Any<CancellationToken>());
        }
    }
}