using System;
using System.ComponentModel.DataAnnotations;

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
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Deadline { get; set; }

        public ProjectStatus Status { get; set; }
    }
}