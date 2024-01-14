using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Timelogger.Dto;
using Timelogger.Entities;

namespace Timelogger.Commands
{
    public record ProjectCommand : IRequest<int>
    {
        public ProjectDto Project { get; set; }
    }

    public class ProjectCommandHandler : IRequestHandler<ProjectCommand, int>
    {
        private readonly TimeloggerDbContext _context;
        private readonly IMapper _mapper;

        public ProjectCommandHandler(TimeloggerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> Handle(ProjectCommand request, CancellationToken cancellationToken)
        {
           var projectEntity = _mapper.Map<Project>(request.Project);

            _context.Projects.Add(projectEntity);
            await _context.SaveChangesAsync(cancellationToken);

            return projectEntity.Id;
        }
    }
}