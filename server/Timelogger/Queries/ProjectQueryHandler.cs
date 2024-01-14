using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Timelogger.Dto;

namespace Timelogger.Queries
{
    public record GetProjectsQuery : IRequest<IEnumerable<ProjectDto>>;
    
    public class ProjectQueryHandler : IRequestHandler<GetProjectsQuery, IEnumerable<ProjectDto>>
    {
        private readonly TimeloggerDbContext _context;
        private readonly IMapper _mapper;

        public ProjectQueryHandler(TimeloggerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _context.Projects.ToListAsync(cancellationToken);
            var projectDtos = _mapper.Map<IEnumerable<ProjectDto>>(projects);

            return projectDtos;
        }
    }
}