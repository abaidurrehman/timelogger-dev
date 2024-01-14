using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Timelogger.Entities;

namespace Timelogger.Commands
{
    public record AddTimeRegistrationCommand : IRequest<CommandResult>
    {
        public int ProjectId { get; set; }
        public int FreelancerId { get; set; }
        public string TaskDescription { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class TimeRegistrationCommandHandler : IRequestHandler<AddTimeRegistrationCommand, CommandResult>
    {
        private readonly TimeloggerDbContext _context;

        public TimeRegistrationCommandHandler(TimeloggerDbContext context)
        {
            _context = context;
        }

        public async Task<CommandResult> Handle(AddTimeRegistrationCommand timeRegistration,
            CancellationToken cancellationToken)
        {
            if (timeRegistration.StartTime >= timeRegistration.EndTime)
                return CommandResult.Fail("End time should be greater than start time.");

            var isDuplicate = await _context.TimeRegistrations.AnyAsync(tr =>
                tr.ProjectId == timeRegistration.ProjectId &&
                tr.StartTime <= timeRegistration.EndTime &&
                tr.EndTime >= timeRegistration.StartTime, cancellationToken: cancellationToken);

            if (isDuplicate) return CommandResult.Fail("Duplicate time registration for the same project.");

            _context.TimeRegistrations.Add(new TimeRegistration
            {
                ProjectId = timeRegistration.ProjectId,
                Date = timeRegistration.Date,
                EndTime = timeRegistration.EndTime,
                StartTime = timeRegistration.StartTime,
                TaskDescription = timeRegistration.TaskDescription,
                FreelancerId = timeRegistration.FreelancerId
            });

            await _context.SaveChangesAsync(cancellationToken);

            return CommandResult.Success("Time registration added successfully.");
        }
    }
}