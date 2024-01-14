using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Timelogger.Entities;

namespace Timelogger.Queries
{
    public record GetTimeRegistrationQueryQuery : IRequest<IEnumerable<TimeRegistration>>
    {
        public int ProjectId { get; set; }
    }

    public class
        TimeRegistrationQueryHandler : IRequestHandler<GetTimeRegistrationQueryQuery, IEnumerable<TimeRegistration>>
    {
        private readonly TimeloggerDbContext _context;

        public TimeRegistrationQueryHandler(TimeloggerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TimeRegistration>> Handle(GetTimeRegistrationQueryQuery request,
            CancellationToken cancellationToken)
        {

            return await _context.TimeRegistrations
                .Where(tr => tr.ProjectId == request.ProjectId)
                .ToListAsync(cancellationToken);
        }
    }
}