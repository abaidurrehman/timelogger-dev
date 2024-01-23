using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timelogger.Entities
{
    public class TimeRegistration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Project")] public int ProjectId { get; set; }

        [ForeignKey("Freelancer")] public int FreelancerId { get; set; }

        public string TaskDescription { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime LoggedDateTime { get; set; } = DateTime.Now;
    }
}