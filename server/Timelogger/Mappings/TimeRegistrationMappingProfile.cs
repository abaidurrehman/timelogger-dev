using AutoMapper;
using Timelogger.Dto;
using Timelogger.Entities;

namespace Timelogger.Mappings
{
    public class TimeRegistrationMappingProfile : Profile
    {
        public TimeRegistrationMappingProfile()
        {
            CreateMap<TimeRegistrationDto, TimeRegistration>();
            CreateMap<TimeRegistration, TimeRegistrationDto>();
        }
    }
}