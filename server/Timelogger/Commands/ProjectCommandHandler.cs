using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Timelogger.Entities;

namespace Timelogger.Commands
{
    public record ProjectCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Deadline { get; set; }
        public ProjectStatus Status { get; set; }
    }

    public class ProjectCommandHandler : IRequestHandler<ProjectCommand, int>
    {
        private readonly TimeloggerDbContext _context;

        public ProjectCommandHandler(TimeloggerDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(ProjectCommand request, CancellationToken cancellationToken)
        {
            var entity = new Project
            {
                Id = request.Id,
                Name = request.Name,
                Status = request.Status,
                Deadline = request.Deadline
            };

            _context.Projects.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}