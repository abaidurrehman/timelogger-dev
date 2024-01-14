using System;

namespace Timelogger.Dto
{
    public class TimeRegistrationDto
    {
        public int? Id { get; set; }
        public int ProjectId { get; set; }
        
        public int FreelancerId { get; set; }
        public string TaskDescription { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}