using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Timelogger.Data.Repositories;
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
        private readonly IMapper _mapper;
        private readonly ITimeRegistrationRepository _timeRegistrationRepository;

        public TimeRegistrationQueryHandler(ITimeRegistrationRepository timeRegistrationRepository, IMapper mapper)
        {
            _timeRegistrationRepository = timeRegistrationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TimeRegistrationDto>> Handle(GetTimeRegistrationQueryQuery request,
            CancellationToken cancellationToken)
        {
            var timeRegistrations =
                await _timeRegistrationRepository.GetTimeRegistrationsForProjectAsync(request.ProjectId,
                    cancellationToken);
            return _mapper.Map<IEnumerable<TimeRegistrationDto>>(timeRegistrations);
        }
    }
}