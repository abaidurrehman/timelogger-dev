using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Timelogger.Dto;

namespace Timelogger.Queries
{
    public record GetTimeRegistrationQueryQuery : IRequest<IEnumerable<TimeRegistrationDto>>
    {
        public int ProjectId { get; set; }
    }

    public class
        TimeRegistrationQueryHandler : IRequestHandler<GetTimeRegistrationQueryQuery, IEnumerable<TimeRegistrationDto>>
    {
        private readonly TimeloggerDbContext _context;
        private readonly IMapper _mapper;

        public TimeRegistrationQueryHandler(TimeloggerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<TimeRegistrationDto>> Handle(GetTimeRegistrationQueryQuery request,
            CancellationToken cancellationToken)
        {
            var timeRegistrations = await _context.TimeRegistrations
                .Where(tr => tr.ProjectId == request.ProjectId)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<TimeRegistrationDto>>(timeRegistrations);
        }
    }
}