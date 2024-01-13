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

public class Freelancer
{
    public int Id { get; set; }

    public string Name { get; set; }
}

public class TimeRegistration
{
    public int? Id { get; set; }
    public int ProjectId { get; set; }

    public int? FreelancerId { get; set; }
    public string TaskDescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}

}