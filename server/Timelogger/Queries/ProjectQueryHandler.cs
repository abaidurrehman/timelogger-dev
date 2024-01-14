using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Timelogger.Entities;

namespace Timelogger.Queries
{
    public record GetProjectsQuery : IRequest<IEnumerable<Project>>;

    public class ProjectQueryHandler : IRequestHandler<GetProjectsQuery, IEnumerable<Project>>
    {
        private readonly TimeloggerDbContext _context;

        public ProjectQueryHandler(TimeloggerDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<Project>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult<IEnumerable<Project>>(_context.Projects);
        }
    }
}