using System;

namespace Timelogger.Entities
{
    public enum ProjectStatus
    {
        New,
        InProgress,
        Complete
    }

    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Deadline { get; set; }

        public ProjectStatus Status { get; set; }
    }
}