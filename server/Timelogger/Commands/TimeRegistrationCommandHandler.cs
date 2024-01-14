using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Timelogger.Data.Repositories;
using Timelogger.Dto;
using Timelogger.Entities;

namespace Timelogger.Commands
{
    public record AddTimeRegistrationCommand : IRequest<CommandResult>
    {
        public TimeRegistrationDto TimeRegistration { get; set; }
    }

    public class TimeRegistrationCommandHandler : IRequestHandler<AddTimeRegistrationCommand, CommandResult>
    {
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepository;
        private readonly ITimeRegistrationRepository _timeRegistrationRepository;

        public TimeRegistrationCommandHandler(ITimeRegistrationRepository timeRegistrationRepository,
            IProjectRepository projectRepository, IMapper mapper)
        {
            _timeRegistrationRepository = timeRegistrationRepository;
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<CommandResult> Handle(AddTimeRegistrationCommand command, CancellationToken cancellationToken)
        {
            var timeRegistrationDto = command.TimeRegistration;
            var timeRegistrationEntity = _mapper.Map<TimeRegistration>(timeRegistrationDto);

            if (timeRegistrationEntity.StartTime >= timeRegistrationEntity.EndTime)
                return CommandResult.Fail("End time should be greater than start time.");

            if ((timeRegistrationEntity.EndTime - timeRegistrationEntity.StartTime).TotalMinutes < 30)
                return CommandResult.Fail("Duration between Start Time and End Time should be at least 30 minutes.");

            if (await _timeRegistrationRepository.IsDuplicateTimeRegistrationAsync(
                    timeRegistrationEntity.ProjectId,
                    timeRegistrationEntity.StartTime,
                    timeRegistrationEntity.EndTime,
                    cancellationToken))
                return CommandResult.Fail("Duplicate time registration for the same project.");

            var project =
                await _projectRepository.GetProjectByIdAsync(timeRegistrationEntity.ProjectId, cancellationToken);

            if (project.Status == ProjectStatus.Complete)
                return CommandResult.Fail("Cannot add time registration to a completed project.");


            await _timeRegistrationRepository.AddTimeRegistrationAsync(timeRegistrationEntity, cancellationToken);

            return CommandResult.Success("Time registration added successfully.");
        }
    }
}