using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Deadline { get; set; }

        public ProjectStatus Status { get; set; }
    }
}