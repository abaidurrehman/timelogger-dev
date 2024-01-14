using System;
using Timelogger.Entities;

namespace Timelogger.Dto
{
    public class ProjectDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Deadline { get; set; }

        public ProjectStatus Status { get; set; }
    }
}