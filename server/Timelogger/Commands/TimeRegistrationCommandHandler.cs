using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
        private readonly TimeloggerDbContext _context;
        private readonly IMapper _mapper;

        public TimeRegistrationCommandHandler(TimeloggerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CommandResult> Handle(AddTimeRegistrationCommand command, CancellationToken cancellationToken)
        {
            var timeRegistrationDto = command.TimeRegistration;
            var timeRegistrationEntity = _mapper.Map<TimeRegistration>(timeRegistrationDto);

            if (timeRegistrationEntity.StartTime >= timeRegistrationEntity.EndTime)
                return CommandResult.Fail("End time should be greater than start time.");

            var isDuplicate = await _context.TimeRegistrations.AnyAsync(tr =>
                tr.ProjectId == timeRegistrationEntity.ProjectId &&
                tr.StartTime <= timeRegistrationEntity.EndTime &&
                tr.EndTime >= timeRegistrationEntity.StartTime, cancellationToken);

            if (isDuplicate) return CommandResult.Fail("Duplicate time registration for the same project.");

            _context.TimeRegistrations.Add(timeRegistrationEntity);
            await _context.SaveChangesAsync(cancellationToken);

            return CommandResult.Success("Time registration added successfully.");
        }
    }
}