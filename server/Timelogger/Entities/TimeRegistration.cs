using System;

namespace Timelogger.Entities
{
    public class TimeRegistration
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public int FreelancerId { get; set; }

        public string TaskDescription { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime LoggedDateTime { get; set; } = DateTime.Now;
    }
}