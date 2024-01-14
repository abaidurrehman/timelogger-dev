using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Timelogger.Data.Repositories;
using Timelogger.Dto;

namespace Timelogger.Queries
{
    public record GetProjectsQuery : IRequest<IEnumerable<ProjectDto>>;

    public class ProjectQueryHandler : IRequestHandler<GetProjectsQuery, IEnumerable<ProjectDto>>
    {
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepository;

        public ProjectQueryHandler(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _projectRepository.GetAllProjectsAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ProjectDto>>(projects);
        }
    }
}