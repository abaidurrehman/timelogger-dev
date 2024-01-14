using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Timelogger.Entities;

namespace Timelogger.Data.Repositories
{
    public interface ITimeRegistrationRepository
    {
        Task<IEnumerable<TimeRegistration>> GetTimeRegistrationsForProjectAsync(int projectId,
            CancellationToken cancellationToken);


        Task<int> AddTimeRegistrationAsync(TimeRegistration timeRegistration, CancellationToken cancellationToken);

        Task<bool> IsDuplicateTimeRegistrationAsync(int projectId, DateTime startTime, DateTime endTime,
            CancellationToken cancellationToken);
    }
}