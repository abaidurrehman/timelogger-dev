using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Timelogger.Entities;

namespace Timelogger.Data.Repositories
{
    public class TimeRegistrationRepository : ITimeRegistrationRepository
    {
        private readonly TimeloggerDbContext _context;

        public TimeRegistrationRepository(TimeloggerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TimeRegistration>> GetTimeRegistrationsForProjectAsync(int projectId,
            CancellationToken cancellationToken)
        {
            return await _context.TimeRegistrations
                .Where(tr => tr.ProjectId == projectId)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> AddTimeRegistrationAsync(TimeRegistration timeRegistration,
            CancellationToken cancellationToken)
        {
            _context.TimeRegistrations.Add(timeRegistration);
            await _context.SaveChangesAsync(cancellationToken);
            return timeRegistration.Id;
        }

        public async Task<bool> IsDuplicateTimeRegistrationAsync(int projectId, DateTime startTime, DateTime endTime,
            CancellationToken cancellationToken)
        {
            return await _context.TimeRegistrations.AnyAsync(tr =>
                tr.ProjectId == projectId &&
                tr.StartTime <= endTime &&
                tr.EndTime >= startTime, cancellationToken);
        }
    }
}