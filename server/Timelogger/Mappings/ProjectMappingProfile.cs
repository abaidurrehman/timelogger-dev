﻿using AutoMapper;
using Timelogger.Dto;
using Timelogger.Entities;

namespace Timelogger.Mappings
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<ProjectDto, Project>();
            CreateMap<Project, ProjectDto>();
        }
    }
}