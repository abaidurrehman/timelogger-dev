using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Timelogger.Data.Repositories;
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
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepository;

        public ProjectCommandHandler(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }
        
        public async Task<int> Handle(ProjectCommand request, CancellationToken cancellationToken)
        {
            var projectEntity = _mapper.Map<Project>(request.Project);
            return await _projectRepository.AddProjectAsync(projectEntity, cancellationToken);
        }
    }
}